using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Main
{
    [Table("job_log")]
    public class JobLog
    {
        [Key] 
        public int Id { get; set; }                 // auto generation
        
        public int JobId { get; set; }
        
        public int Level { get; set; }
        
        public DateTime? Time { get; set; } = null;
        
        public string Message { get; set; }
        
        public string? Exception { get; set; } = null;       // json
        
        public string Properties { get; set; }              // json
        
        [ForeignKey("JobId")]
        public Job? Job { get; set; } = null;
    }
}