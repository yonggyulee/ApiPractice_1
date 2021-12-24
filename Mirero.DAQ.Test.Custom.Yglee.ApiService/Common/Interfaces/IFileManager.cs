using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Common.Interfaces
{
    public interface IFileManager
    {
        void SaveFile(IFormFile imgFile, string path);
        bool RemoveFile(string path); 
        Task<byte[]?> LoadFile(string path);
    }
}
