using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Storage
{
    public class ObjectDetectionLabelDTO
    {
        // TODO Id와 Id 중 어떤 단어를 쓸것인지 항상 혼동이 있습니다. 어떤것이 바람직 할까요.
        public int Id { get; set; }
        
        public string LabelSetId { get; set; }
        
        public string SampleId { get; set; }
        
        public string ImageId { get; set; }
        
        public string LabelPath { get; set; }
    }
}