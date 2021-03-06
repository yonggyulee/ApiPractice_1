using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Storage
{
    public class SegmentationLabelDTO
    {
        public int Id { get; set; }

        public string LabelSetId { get; set; }

        public string SampleId { get; set; }

        public string ImageId { get; set; }

        public string LabelPath { get; set; }
    }
}