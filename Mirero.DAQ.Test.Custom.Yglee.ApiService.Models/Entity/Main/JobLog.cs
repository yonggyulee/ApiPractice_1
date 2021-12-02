using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Main
{
    [Table("job_log")]
    public class JobLog
    {
        [Key] 
        [Column("id")] 
        public int ID { get; set; }                 // auto generation
        
        [Column("job_id")]
        public int JobID { get; set; }
        
        [Column("level")]
        public int Level { get; set; }
        
        [Column("time")]
        public DateTime? Time { get; set; } = null;
        
        [Column("message")]
        public string Message { get; set; }
        
        [Column("exception")]
        public string? Exception { get; set; } = null;       // json
        
        [Column("properties")]
        public string Properties { get; set; }              // json
        
        [ForeignKey("JobID")]
        public Job? Job { get; set; } = null;
    }
}