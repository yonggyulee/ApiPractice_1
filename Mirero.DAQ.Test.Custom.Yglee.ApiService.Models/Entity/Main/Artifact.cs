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
        public int Id { get; set; }                // auto generation
        
        public int JobId { get; set; }
        
        public string Name { get; set; }
        
        public string VolumeId { get; set; }
        
        public string Uri { get; set; }

        public string? Descriptions { get; set; } = null;
        
        [ForeignKey("JobId")]
        public Job? Job { get; set; } = null;
        
        [ForeignKey("VolumeId")]
        public Volume? Volume { get; set; } = null;
    }
}