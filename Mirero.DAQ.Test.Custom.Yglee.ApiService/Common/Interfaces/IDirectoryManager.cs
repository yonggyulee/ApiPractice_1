using System.Collections.Generic;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Common.Interfaces
{
    public interface IDirectoryManager
    {
        void CreateDirectory(string uri);
        void DeleteDirectory(string uri);
        List<string> GetDatasetList();
    }
}