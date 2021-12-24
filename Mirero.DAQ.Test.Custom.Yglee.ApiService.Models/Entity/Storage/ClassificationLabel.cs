using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Storage;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage
{
    [Table("classification_label")]
    public class ClassificationLabel
    {
        [Key]
        public int Id { get; set; }
        
        public string LabelSetId { get; set; }
        
        public string SampleId { get; set; }
        
        public string ImageId { get; set; }
        
        public int ClassCodeId { get; set; }
        
        [ForeignKey("LabelSetId")]
        public LabelSet? LabelSet { get; set; } = null;
        
        [ForeignKey("SampleId")]
        public Sample? Sample { get; set; } = null;
        
        [ForeignKey("ImageId")]
        public Image? Image { get; set; } = null;
    }
}