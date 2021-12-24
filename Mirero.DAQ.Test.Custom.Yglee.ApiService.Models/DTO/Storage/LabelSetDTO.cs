using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Storage
{
    public class LabelSetDTO
    {
        public string Id { get; set; }
        
        public int ClassCodeSetId { get; set; }
        
        public string Descriptions { get; set; }
    }
}