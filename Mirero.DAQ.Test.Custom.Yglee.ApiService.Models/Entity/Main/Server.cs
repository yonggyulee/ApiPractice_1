using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Main
{
    [Table("server")]
    public class Server
    {
        [Key]
        [Column("id")]
        public string ID { get; set; }
        
        [Column("ip")]
        public string IP { get; set; }
        
        [Column("port")]
        public int Port { get; set; }
        
        [Column("server_type")]
        public string ServerType { get; set; }
        
        [Column("os_type")]
        public string OSType { get; set; }
        
        [Column("os_version")]
        public string OSVersion { get; set; }
        
        [Column("cpu_count")]
        public int CPUCount { get; set; }
        
        [Column("cpu_memory")]
        public int CPUMemory { get; set; }
        
        [Column("gpu_name")]
        public string? GPUName { get; set; } = null;
        
        [Column("gpu_count")]
        public int? GPUCount { get; set; } = null;
        
        [Column("gpu_memory")]
        public int? GPUMemory { get; set; } = null;

        public ICollection<Worker>? Workers { get; set; } = new List<Worker>();
    }
}