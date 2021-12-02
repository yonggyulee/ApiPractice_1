using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Storage
{
    public class SampleDTO
    {
        public string ID { get; set; }
        
        public int DatasetID { get; set; }

        public int ImageCount { get; set; }
        
        public string URI { get; set; }
    }
}