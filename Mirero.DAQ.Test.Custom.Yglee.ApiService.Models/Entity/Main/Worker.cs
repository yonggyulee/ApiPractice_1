using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Main
{
    [Table("worker")]
    public class Worker
    {
        [Key]
        [Column("id")]
        public string ID { get; set; }
        
        [Column("server_id")]
        public string ServerID { get; set; }
        
        [Column("worker_type")]
        public string WorkerType { get; set; }
        
        [Column("properties")]
        public string? Properties { get; set; } = null;    // json
        
        [Column("cpu_count")]
        public int? CPUCount { get; set; } = null;
        
        [Column("cpu_memory")]
        public int? CPUMemory { get; set; } = null;          // KB
        
        [Column("gpu_count")]
        public int? GPUCount { get; set; } = null;
        
        [Column("gpu_memory")]
        public int? GPUMemory { get; set; } = null;          // KB
        
        [ForeignKey("ServerID")]
        public Server? Server { get; set; } = null;
    }
}