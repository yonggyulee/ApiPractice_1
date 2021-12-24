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
        public string Id { get; set; }
        
        public string SampleId { get; set; }
        
        public string ImageCode { get; set; }
        
        public string OriginalFilename { get; set; }
        
        public int Width { get; set; }
        
        public int Height { get; set; }
        
        public int Channel { get; set; }
        
        public string Dtype { get; set; }
        
        public string Path { get; set; }               
        
        [NotMapped]
        public IFormFile? ImageFile { get; set; } = null;
        
        [ForeignKey("SampleId")]
        public Sample? Sample { get; set; } = null;

        public ICollection<ClassificationLabel>? ClassificationLabels { get; set; } = new List<ClassificationLabel>();

        public ICollection<ObjectDetectionLabel>? ObjectDetectionLabels { get; set; } = new List<ObjectDetectionLabel>();

        public ICollection<SegmentationLabel>? SegmentationLabels { get; set; } = new List<SegmentationLabel>();
    }
}