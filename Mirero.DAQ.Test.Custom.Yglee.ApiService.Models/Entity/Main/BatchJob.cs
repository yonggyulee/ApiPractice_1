using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Main;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Main
{
    [Table("batchjob")]
    public class BatchJob
    {
        [Key]
        public int Id { get; set; }
        
        public string Type { get; set; }
        
        public int? TotalCount { get; set; }
        
        public int? EndCount { get; set; }
        
        public DateTime? ReadyStart { get; set; } = null;
        
        public DateTime? ProgressStart { get; set; } = null;
        
        public DateTime? ProgressEnd { get; set; } = null;

        public ICollection<Job>? Jobs { get; set; } = new List<Job>();
    }
}