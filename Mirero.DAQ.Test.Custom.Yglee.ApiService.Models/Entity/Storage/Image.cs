using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Storage;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage
{
    [Table("image")]
    public class Image
    {
        [Key]
        [Column("id")]
        public string ID { get; set; }
        
        [Column("sample_id")]
        public string SampleID { get; set; }
        
        [Column("image_code")]
        public string ImageCode { get; set; }

        [Column("original_filename")]
        public string OriginalFilename { get; set; }

        [Column("width")]
        public int Width { get; set; }
        
        [Column("height")]
        public int Height { get; set; }
        
        [Column("channel")]
        public int Channel { get; set; }
        
        [Column("dtype")]
        public string Dtype { get; set; }
        
        [Column("path")]
        public string Path { get; set; }               
        
        [NotMapped]
        public IFormFile? ImageFile { get; set; } = null;
        
        [ForeignKey("SampleID")]
        public Sample? Sample { get; set; } = null;

        public ICollection<ClassificationLabel>? ClassificationLabels { get; set; } = new List<ClassificationLabel>();

        public ICollection<ObjectDetectionLabel>? ObjectDetectionLabels { get; set; } = new List<ObjectDetectionLabel>();

        public ICollection<SegmentationLabel>? SegmentationLabels { get; set; } = new List<SegmentationLabel>();
    }
}