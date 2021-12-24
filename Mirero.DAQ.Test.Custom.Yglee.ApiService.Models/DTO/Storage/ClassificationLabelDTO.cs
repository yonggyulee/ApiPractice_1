using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Storage
{
    public class ClassificationLabelDTO
    {
        public int Id { get; set; }
        
        public string LabelSetId { get; set; }
        
        public string SampleId { get; set; }
        
        public string ImageId { get; set; }
        
        public int ClassCodeId { get; set; }
    }
}