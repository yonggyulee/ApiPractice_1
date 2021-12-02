using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Main;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Main
{
    [Table("batch_job")]
    public class BatchJob
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        
        [Column("type")]
        public string Type { get; set; }
        
        [Column("total_count")]
        public int? TotalCount { get; set; }
        
        [Column("end_count")]
        public int? EndCount { get; set; }
        
        [Column("ready_start")]
        public DateTime? ReadyStart { get; set; } = null;
        
        [Column("progress_start")]
        public DateTime? ProgressStart { get; set; } = null;
        
        [Column("progress_end")]
        public DateTime? ProgressEnd { get; set; } = null;

        public ICollection<Job>? Jobs { get; set; } = new List<Job>();
    }
}