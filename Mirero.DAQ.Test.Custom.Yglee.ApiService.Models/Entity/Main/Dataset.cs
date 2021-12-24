using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Main;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Main
{
    [Table("dataset")]
    public class Dataset
    {
        [Key]
        public int Id { get; set; }                
        
        public string Name { get; set; }
        
        public string VolumeId { get; set; }
        
        public string Uri { get; set; }
        
        public string? Descriptions { get; set; } = null;
        
        [ForeignKey("VolumeId")]
        public Volume? Volume { get; set; } = null;
    }
}