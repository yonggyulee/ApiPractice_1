using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Storage;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage
{
    [Table("object_detection_label")]
    public class ObjectDetectionLabel
    {
        [Key]
        public int Id { get; set; }
        
        public string LabelSetId { get; set; }
        
        public string SampleId { get; set; }
        
        public string ImageId { get; set; }
        
        public string LabelPath { get; set; }       // json path
        
        [ForeignKey("LabelSetId")]
        public LabelSet? LabelSet { get; set; } = null;
        
        [ForeignKey("SampleId")]
        public Sample? Sample { get; set; } = null;
        
        [ForeignKey("ImageId")]
        public Image? Image { get; set; } = null;
    }
}