using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Services.Utils
{
    public interface IFileManager
    {
        void SaveFile(IFormFile imgFile, string path);
        bool RemoveFile(string path); 
        Task<byte[]?> LoadFile(string path);
    }

    public class FileManager : IFileManager
    {
        public void SaveFile(IFormFile imgFile, string path)
        {
            // 경로 검증
            Console.WriteLine($"Path : {path}");
            string folderPath = path.Substring(0, path.LastIndexOf('\\'));
            Console.WriteLine($"FolderPath : {folderPath}");

            DirectoryInfo di = new DirectoryInfo(folderPath);
            if (di.Exists == false)
            {
                di.Create();
            }

            if (imgFile.Length > 0)
            {
                using (var imgStream = imgFile.OpenReadStream())
                {
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        imgStream.CopyTo(fileStream);
                    }
                }
            }
        }

        public bool RemoveFile(string path)
        {
            if (System.IO.File.Exists(path))
            {
                try
                {
                    System.IO.File.Delete(path);
                }
                catch (IOException)
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<byte[]?> LoadFile(string path)
        {
            if (System.IO.File.Exists(path))
            {
                byte[]? bytes;
                using (FileStream file = new FileStream(path, FileMode.Open))
                {
                    try
                    {
                        bytes = new byte[file.Length];
                        await file.ReadAsync(bytes);

                        return bytes;
                    }
                    catch (Exception)
                    {
                        throw new Exception($"ERROR:LoadFile{path}");
                    }
                }
            }
            return null;
        }
    }
}
