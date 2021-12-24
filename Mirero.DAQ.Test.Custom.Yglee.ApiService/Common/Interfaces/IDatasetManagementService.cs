using System.Collections.Generic;
using System.Threading.Tasks;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Main;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Common.Interfaces
{
    public interface IDatasetManagementService
    {
        Task<DatasetDTO> CreateDatasetAsync(DatasetDTO dataset);
        Task<DatasetDTO?> RemoveDatasetAsync(int id);
        Task<List<DatasetDTO>> ToListAsync();
        Task<DatasetDTO?> FindAsync(int id);
        Task<int> UpdateDataset(int id, DatasetDTO dataset);
    }

    
}