using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Storage;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage
{
    [Table("segmentation_label")]
    public class SegmentationLabel
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        
        [Column("label_set_id")]
        public string LabelSetID { get; set; }
        
        [Column("sample_id")]
        public string SampleID { get; set; }
        
        [Column("image_id")]
        public string ImageID { get; set; }
        
        [Column("label_path")]
        public string LabelPath { get; set; }       // image path
        
        [ForeignKey("LabelSetID")]
        public LabelSet? LabelSet { get; set; } = null;
        
        [ForeignKey("SampleID")]
        public Sample? Sample { get; set; } = null;
        
        [ForeignKey("ImageID")]
        public Image? Image { get; set; } = null;
    }
}