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
        [Column("id")]
        public int ID { get; set; }                
        
        [Column("name")]
        public string Name { get; set; }
        
        [Column("volume_id")]
        public string VolumeID { get; set; }
        
        [Column("uri")]
        public string URI { get; set; }
        
        [Column("descriptions")]
        public string? Descriptions { get; set; } = null;
        
        [ForeignKey("VolumeID")]
        public Volume? Volume { get; set; } = null;
    }
}