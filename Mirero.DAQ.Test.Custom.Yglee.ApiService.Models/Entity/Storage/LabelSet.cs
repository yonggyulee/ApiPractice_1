using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Storage;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage
{
    [Table("label_set")]
    public class LabelSet
    {
        [Key]
        [Column("id")]
        public string ID { get; set; }
        
        [Column("class_code_set_id")]
        public int ClassCodeSetID { get; set; }
        
        [Column("descriptions")]
        public string Descriptions { get; set; }

        public ICollection<ClassificationLabel> ClassificationLabels { get; set; } = new List<ClassificationLabel>();

        public ICollection<ObjectDetectionLabel> ObjectDetectionLabels { get; set; } = new List<ObjectDetectionLabel>();

        public ICollection<SegmentationLabel> SegmentationLabels { get; set; } = new List<SegmentationLabel>();
    }
}