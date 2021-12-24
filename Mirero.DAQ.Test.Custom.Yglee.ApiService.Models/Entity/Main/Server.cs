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
        public string Id { get; set; }
        
        public string IP { get; set; }
        
        public int Port { get; set; }
        
        public string ServerType { get; set; }
        
        public string OSType { get; set; }
        
        public string OSVersion { get; set; }
        
        public int CpuCount { get; set; }
        
        public int CpuMemory { get; set; }
        
        public string? GpuName { get; set; } = null;
        
        public int? GpuCount { get; set; } = null;
        
        public int? GpuMemory { get; set; } = null;

        public ICollection<Worker>? Workers { get; set; } = new List<Worker>();
    }
}