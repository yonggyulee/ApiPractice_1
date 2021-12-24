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
        public int Id { get; set; }                    // auto generation
        
        public int BatchJobId { get; set; }
        
        public string WorkerId { get; set; }
        
        public string Status { get; set; }
        
        public DateTime? ReadyStart { get; set; } = null;
        
        public DateTime? ProgressStart { get; set; } = null;
        
        public DateTime? ProgressEnd { get; set; } = null;
        
        [ForeignKey("BatchJobId")]
        public BatchJob? BatchJob { get; set; } = null;

        public ICollection<Artifact>? Artifacts { get; set; } = new List<Artifact>();

        public ICollection<JobLog>? JobLogs { get; set; } = new List<JobLog>();
    }
}