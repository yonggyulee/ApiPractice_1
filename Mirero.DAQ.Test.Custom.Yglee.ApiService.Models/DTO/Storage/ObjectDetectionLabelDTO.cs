using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Storage
{
    public class ObjectDetectionLabelDTO
    {
        public int ID { get; set; }
        
        public string LabelSetID { get; set; }
        
        public string SampleID { get; set; }
        
        public string ImageID { get; set; }
        
        public string LabelPath { get; set; }
    }
}