using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Storage;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage
{
    [Table("sample")]
    public class Sample
    {
        [Key]
        [Column("id")]
        public string ID { get; set; }
        
        [Column("dataset_id")]
        public int DatasetID { get; set; }

        [Column("image_count")]
        public int ImageCount { get; set; }
        
        [Column("uri")]
        public string URI { get; set; }

        public ICollection<Image> Images { get; set; } = new List<Image>();

        public ICollection<ClassificationLabel> ClassificationLabels { get; set; } = new List<ClassificationLabel>();

        public ICollection<ObjectDetectionLabel> ObjectDetectionLabels { get; set; } = new List<ObjectDetectionLabel>();

        public ICollection<SegmentationLabel> SegmentationLabels { get; set; } = new List<SegmentationLabel>();
        
        // public bool ShouldSerializeMetadata()
        // {
        //     return !String.IsNullOrWhiteSpace(this.Metadata);
        // }
    }
}