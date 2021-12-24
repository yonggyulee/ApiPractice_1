using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Storage;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Common.Interfaces
{
    public interface IImageManagementService
    {
        Task<List<ImageDTO>> ToListAsync(string dsUri);
        Task<ImageDTO> FindAsync(string dsUri, string id);
        Task<int> UpdateAsync(string dsUri, string id, ImageDTO image);
        Task<ImageDTO?> AddAsync(string dsUri, ImageDTO image);
        Task<ImageDTO?> RemoveAsync(string dsUri, string id);
        void Upload(string dsUri, string id, IFormFile imageFile);
        Task<byte[]?> GetImageFileAsync(string dsUri, string id);
    }
}
