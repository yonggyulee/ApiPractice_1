using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Context;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Main;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Main;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Services.Utils;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Services
{
    public interface IDatasetManagementService
    {
        Task<Dataset> CreateDatasetAsync(Dataset dataset);

        Task<Dataset?> RemoveDatasetAsync(int id);
        
        Task<List<Dataset>> ToListAsync();

        Task<Dataset?> FindAsync(int id);

        Task<int> UpdateDataset(int id, Dataset dataset);
    }

    class DatasetManagementService : IDatasetManagementService
    {
        private readonly DatasetDbContext _datasetDbContext;
        private readonly IDatasetDbContextFactory _datasetDbContextFactory;
        private readonly MainDbContext _context;
        private readonly IDirectoryManager _directoryManager;

        public DatasetManagementService(IDatasetDbContextFactory datasetDbContextFactory, MainDbContext context, IDirectoryManager directoryManager)
        {
            _datasetDbContextFactory = datasetDbContextFactory;
            _context = context;
            _directoryManager = directoryManager;
        }

        public async Task<Dataset> CreateDatasetAsync(Dataset dataset)
        {
            if (dataset.ID != null)
            {
                // 기존 데이터셋 중 생성하려는 데이터셋이 존재하는 확인
                if (DatasetExists(dataset.ID))
                {
                    //return BadRequest($"Dataset(id:{datasetDto.ID}) already exist.");
                    throw new InvalidOperationException($"Dataset(id:{dataset.ID}) already exist.");
                }
            }

            if (_directoryManager.GetDatasetList().Contains(dataset.URI))
            {
                //return BadRequest($"Dataset(uri:{datasetDto.URI}) already exist.");
                throw new InvalidOperationException($"Dataset(uri:{dataset.URI}) already exist.");
            }

            try
            {
                // 데이터셋 디렉토리 생성
                _directoryManager.CreateDirectory(dataset.URI);

                // db 생성
                await using (var db = _datasetDbContextFactory.CreateContext(dataset.URI))
                {
                    // 데이터베이스 마이그레이션
                    await db.MigrateDb();
                }

                _context.Datasets.Add(dataset);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _directoryManager.DeleteDirectory(dataset.URI);
                Console.WriteLine(e);
                throw new InvalidOperationException($"ERROR:{e}");
            }

            return await _context.Datasets
                .FirstAsync(d => d.Name == dataset.Name && d.URI == d.URI && d.VolumeID == d.VolumeID); ;
        }

        public async Task<Dataset?> RemoveDatasetAsync(int id)
        {
            var dataset = await _context.Datasets.FindAsync(id);
            if (dataset == null)
            {
                return dataset;
            }
            
            _context.Datasets.Remove(dataset);
            await _context.SaveChangesAsync();
            
            // 기존 데이터셋 중 삭제하려는 데이터셋 디렉토리가 존재하는 확인
            if (_directoryManager.GetDatasetList().Contains(dataset.URI))
            {
                _directoryManager.DeleteDirectory(dataset.URI);
            }

            return dataset;
        }

        public async Task<List<Dataset>> ToListAsync()
        {
            return await _context.Datasets.ToListAsync();
        }

        public async Task<Dataset?> FindAsync(int id)
        {
            var dataset = await _context.Datasets.FindAsync(id);
            
            return dataset;
        }

        public async Task<int> UpdateDataset(int id, Dataset dataset)
        {
            _context.Entry(dataset).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DatasetExists(id))
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            return 1;
        }

        private bool DatasetExists(int id)
        {
            return _context.Datasets.Any(e => e.ID == id);
        }
    }
}