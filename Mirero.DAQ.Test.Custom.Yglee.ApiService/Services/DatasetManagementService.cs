using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Medallion.Threading.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Common.Interfaces;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Context;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Main;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Main;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Services
{
    class DatasetManagementService : IDatasetManagementService
    {
        private readonly IDatasetDbContextFactory _dsDbContextFactory;
        private readonly MainDbContext _context;
        private readonly IDirectoryManager _directoryManager;
        private readonly IConfiguration _configuration;

        public DatasetManagementService(IDatasetDbContextFactory dsDbContextFactory, MainDbContext context, IDirectoryManager directoryManager, IConfiguration configuration)
        {
            _dsDbContextFactory = dsDbContextFactory;
            _context = context;
            _directoryManager = directoryManager;
            _configuration = configuration;
        }

        public async Task<DatasetDTO> CreateDatasetAsync(DatasetDTO datasetDto)
        {
            var dataset = datasetDto.Adapt<Dataset>();

            if (dataset.Id != null)
            {
                // 기존 데이터셋 중 생성하려는 데이터셋이 존재하는 확인
                if (DatasetExists(dataset.Id))
                {
                    //return BadRequest($"Dataset(id:{datasetDto.ID}) already exist.");
                    throw new InvalidOperationException($"Dataset(id:{dataset.Id}) already exist.");
                }
            }

            if (_directoryManager.GetDatasetList().Contains(dataset.Uri))
            {
                //return BadRequest($"Dataset(uri:{datasetDto.URI}) already exist.");
                throw new InvalidOperationException($"Dataset(uri:{dataset.Uri}) already exist.");
            }

            try
            {
                // 데이터셋 디렉토리 생성
                _directoryManager.CreateDirectory(dataset.Uri);

                // db 생성
                await using (var db = _dsDbContextFactory.CreateContext(dataset.Uri))
                {
                    // 데이터베이스 마이그레이션
                    await db.MigrateDb();
                }

                _context.Datasets.Add(dataset);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _directoryManager.DeleteDirectory(dataset.Uri);
                Console.WriteLine(e);
                throw new InvalidOperationException($"ERROR:{e}");
            }

            dataset = await _context.Datasets
                .FirstAsync(d => d.Name == dataset.Name && d.Uri == dataset.Uri && d.VolumeId == dataset.VolumeId);

            return dataset.Adapt<DatasetDTO>();
        }

        public async Task<DatasetDTO?> RemoveDatasetAsync(int id)
        {
            var dataset = await _context.Datasets.FindAsync(id);
            if (dataset == null)
            {
                return null;
            }

            _context.Datasets.Remove(dataset);
            await _context.SaveChangesAsync();

            // 기존 데이터셋 중 삭제하려는 데이터셋 디렉토리가 존재하는 확인
            if (_directoryManager.GetDatasetList().Contains(dataset.Uri))
            {
                _directoryManager.DeleteDirectory(dataset.Uri);
            }

            return dataset.Adapt<DatasetDTO>();
        }

        public async Task<List<DatasetDTO>> ToListAsync()
        {
            return await _context.Datasets.Select(d => d.Adapt<DatasetDTO>()).ToListAsync();
        }

        public async Task<DatasetDTO?> FindAsync(int id)
        {
            var dataset = await _context.Datasets.FindAsync(id);

            return dataset.Adapt<DatasetDTO>();
        }

        public async Task<int> UpdateDataset(int id, DatasetDTO datasetDto)
        {
            var dataset = datasetDto.Adapt<Dataset>();
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
            return _context.Datasets.Any(e => e.Id == id);
        }
    }
}
