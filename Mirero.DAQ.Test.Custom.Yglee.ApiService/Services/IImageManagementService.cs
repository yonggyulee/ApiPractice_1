using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Context;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Services.Utils;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Services
{
    public interface IImageManagementService
    {
        Task<List<Image>> ToListAsync(string dsUri);
        Task<Image> FindAsync(string dsUri, string id);
        Task<int> UpdateAsync(string dsUri, string id, Image image);
        Task<Image?> AddAsync(string dsUri, Image image);
        Task<Image?> RemoveAsync(string dsUri, string id);
        void Upload(string dsUri, string id, IFormFile imageFile);
        Task<byte[]?> GetImageFileAsync(string dsUri, string id);
    }

    public class ImageManagementService : IImageManagementService
    {
        private readonly IDatasetDbContextFactory _datasetcontextContextFactory;
        private readonly IFileManager _fileManager;

        public ImageManagementService(IDatasetDbContextFactory datasetcontextContextFactory, IFileManager fileManager)
        {
            _datasetcontextContextFactory = datasetcontextContextFactory;
            _fileManager = fileManager;
        }

        public async Task<List<Image>> ToListAsync(string dsUri)
        {
            await using var context = _datasetcontextContextFactory.CreateContext(dsUri);
            return await context.Images.ToListAsync();
        }

        public async Task<Image> FindAsync(string dsUri, string id)
        {
            await using var context = _datasetcontextContextFactory.CreateContext(dsUri);
            return await context.Images.FindAsync(id);
        }

        public async Task<int> UpdateAsync(string dsUri, string id, Image image)
        {
            await using var context = _datasetcontextContextFactory.CreateContext(dsUri);
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

        public async Task<Image?> AddAsync(string dsUri, Image image)
        {
            await using var context = _datasetcontextContextFactory.CreateContext(dsUri);
            var sample = await context.Samples.FindAsync(image.SampleID);
            if (sample == null)
            {
                return null;
            }

            sample.Images.Add(image);
            sample.ImageCount++;

            await context.SaveChangesAsync();

            return await context.Images
                .FindAsync(image.ID);
        }

        public async Task<Image?> RemoveAsync(string dsUri, string id)
        {
            await using var context = _datasetcontextContextFactory.CreateContext(dsUri);

            var image = await context.Images.FindAsync(id);
            if (image == null)
            {
                return image;
            }

            var sample = await context.Samples.FindAsync(image.SampleID);
            if (sample != null) sample.ImageCount--;

            context.Images.Remove(image);

            string currentPath =
                Path.Combine(Environment.CurrentDirectory, image.Path);

            var verifiedPath = PathToVerifiedPath(currentPath);

            _fileManager.RemoveFile(verifiedPath);

            await context.SaveChangesAsync();

            return image;
        }

        public void Upload(string dsUri, string id, IFormFile imageFile)
        {
            var verifiedId = IdToVerifiedId(id);
            Console.WriteLine($"UPLOADIMAGEFILE : {verifiedId}");

            if (imageFile == null)
            {
                Console.WriteLine("ImageFile is null.");
                //return NotFound("imageFile is null.");
            }

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
            Console.WriteLine($"DownloadImage Find Success : {dsUri} {verifiedId} {image.ID}");


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
            return context.Images.Any(e => e.ID == id);
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
