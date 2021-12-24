using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Storage
{
    public class SampleDTO
    {
        public string Id { get; set; }
        
        public int DatasetId { get; set; }

        public int ImageCount { get; set; }
        
        public string Uri { get; set; }
    }
}