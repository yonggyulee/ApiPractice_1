using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Main;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Main
{
    [Table("job")]
    public class Job
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }                    // auto generation
        
        [Column("batch_job_id")]
        public int BatchJobID { get; set; }
        
        [Column("worker_id")]
        public string WorkerID { get; set; }
        
        [Column("status")]
        public string Status { get; set; }
        
        [Column("ready_start")]
        public DateTime? ReadyStart { get; set; } = null;
        
        [Column("progress_start")]
        public DateTime? ProgressStart { get; set; } = null;
        
        [Column("progress_end")]
        public DateTime? ProgressEnd { get; set; } = null;
        
        [ForeignKey("BatchJobID")]
        public BatchJob? BatchJob { get; set; } = null;

        public ICollection<Artifact>? Artifacts { get; set; } = new List<Artifact>();

        public ICollection<JobLog>? JobLogs { get; set; } = new List<JobLog>();
    }
}