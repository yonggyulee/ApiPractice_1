using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Medallion.Threading.Postgres;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Common.Interfaces;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Context;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Storage;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Services
{
    public class ImageManagementService : IImageManagementService
    {
        private readonly IDatasetDbContextFactory _dsDbContextFactory;
        private readonly IFileManager _fileManager;

        public ImageManagementService(IDatasetDbContextFactory dsDbContextFactory, IFileManager fileManager)
        {
            _dsDbContextFactory = dsDbContextFactory;
            _fileManager = fileManager;
        }

        public async Task<List<ImageDTO>> ToListAsync(string dsUri)
        {
            await using var context = _dsDbContextFactory.CreateContext(dsUri);
            return await context.Images.Select(i => i.Adapt<ImageDTO>()).ToListAsync();
        }

        public async Task<ImageDTO> FindAsync(string dsUri, string id)
        {
            await using var context = _dsDbContextFactory.CreateContext(dsUri);
            var image = await context.Images.FindAsync(id);
            return image.Adapt<ImageDTO>();
        }

        public async Task<int> UpdateAsync(string dsUri, string id, ImageDTO imageDto)
        {
            //var lock = new PostgresDistributedLock(new PostgresAdvisoryLockKey("", allowHashing : true));

            var image = imageDto.Adapt<Image>();

            await using var context = _dsDbContextFactory.CreateContext(dsUri);
            context.Entry(image).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImageExists(context, id))
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

        public async Task<ImageDTO?> AddAsync(string dsUri, ImageDTO imageDto)
        {
            var image = imageDto.Adapt<Image>();

            await using var context = _dsDbContextFactory.CreateContext(dsUri);
            var sample = await context.Samples.FindAsync(image.SampleId);
            if (sample == null)
            {
                return null;
            }

            sample.Images.Add(image);
            sample.ImageCount++;

            await context.SaveChangesAsync();

            image = await context.Images
                                    .FindAsync(image.Id);

            return image.Adapt<ImageDTO>();
        }

        public async Task<ImageDTO?> RemoveAsync(string dsUri, string id)
        {
            await using var context = _dsDbContextFactory.CreateContext(dsUri);

            var image = await context.Images.FindAsync(id);
            if (image == null)
            {
                return null;
            }

            var sample = await context.Samples.FindAsync(image.SampleId);
            if (sample != null) sample.ImageCount--;

            context.Images.Remove(image);

            string currentPath =
                Path.Combine(Environment.CurrentDirectory, image.Path);

            var verifiedPath = PathToVerifiedPath(currentPath);

            _fileManager.RemoveFile(verifiedPath);

            await context.SaveChangesAsync();

            return image.Adapt<ImageDTO>();
        }

        public void Upload(string dsUri, string id, IFormFile imageFile)
        {
            var verifiedId = IdToVerifiedId(id);
            Console.WriteLine($"UPLOADIMAGEFILE : {verifiedId}");

            if (imageFile != null)
            {
                Console.WriteLine($"IMAGEFILE SAVE : {verifiedId}");
                // 이미지 파일을 저장할 폴더 경로
                string currentPath =
                    Path.Combine(Environment.CurrentDirectory, "database", dsUri, "images");

                var verifiedPath = PathToVerifiedPath(Path.Combine(currentPath, verifiedId));

                try
                {
                    // 파일 저장
                    _fileManager.SaveFile(imageFile, verifiedPath);
                }
                catch (IOException e)
                {
                    throw e;
                }
            }
        }

        public async Task<byte[]?> GetImageFileAsync(string dsUri, string id)
        {
            await using var context = DatasetDbContext.GetInstance(dsUri);

            Console.WriteLine($"DownloadImage : {dsUri} {id}");

            // 이미지 조회
            string verifiedId = IdToVerifiedId(id);
            Console.WriteLine($"DownloadImage : {dsUri} {verifiedId}");
            var image = await context.Images.FindAsync(verifiedId);
            Console.WriteLine($"DownloadImage Find Success : {dsUri} {verifiedId} {image.Id}");


            // 이미지 파일 경로
            string currentPath =
                Path.Combine(Environment.CurrentDirectory, image.Path);

            var verifiedPath = PathToVerifiedPath(Path.Combine(currentPath));

            Console.WriteLine($"DownloadImage Verified_path : {verifiedPath}");
            // 파일 반환
            return await _fileManager.LoadFile(verifiedPath);
        }

        private bool ImageExists(DatasetDbContext context, string id)
        {
            return context.Images.Any(e => e.Id == id);
        }

        private string PathToVerifiedPath(string path)
        {
            return path.Replace('/', '\\');
        }

        private string IdToVerifiedId(string id)
        {
            return id.Replace("%2F", "/");
        }
    }
}
