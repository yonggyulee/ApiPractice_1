using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Main;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Main
{
    [Table("artifact")]
    public class Artifact
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }                // auto generation
        
        [Column("job_id")]
        public int JobID { get; set; }
        
        [Column("name")]
        public string Name { get; set; }
        
        [Column("volume_id")]
        public string VolumeID { get; set; }
        
        [Column("uri")]
        public string URI { get; set; }

        [Column("descriptions")] public string? Descriptions { get; set; } = null;
        
        [ForeignKey("JobID")]
        public Job? Job { get; set; } = null;
        
        [ForeignKey("VolumeID")]
        public Volume? Volume { get; set; } = null;
    }
}