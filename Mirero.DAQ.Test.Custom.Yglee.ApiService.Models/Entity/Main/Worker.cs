using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Main
{
    [Table("worker")]
    public class Worker
    {
        [Key]
        public string Id { get; set; }
        
        public string ServerId { get; set; }
        
        public string WorkerType { get; set; }
        
        public string? Properties { get; set; } = null;    // json
        
        public int? CpuCount { get; set; } = null;
        
        public int? CpuMemory { get; set; } = null;          // KB
        
        public int? GpuCount { get; set; } = null;
        
        public int? GpuMemory { get; set; } = null;          // KB
        
        [ForeignKey("ServerId")]
        public Server? Server { get; set; } = null;
    }
}