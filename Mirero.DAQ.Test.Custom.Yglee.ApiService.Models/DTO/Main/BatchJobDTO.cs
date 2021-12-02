using System;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Main;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Main
{
    public class BatchJobDTO
    {
        public int ID { get; set; }
        
        public string Type { get; set; }
        
        public int? TotalCount { get; set; }
        
        public int? EndCount { get; set; }
        
        public DateTime? ReadyStart { get; set; }
        
        public DateTime? ProgressStart { get; set; }
        
        public DateTime? ProgressEnd { get; set; }
    }
}