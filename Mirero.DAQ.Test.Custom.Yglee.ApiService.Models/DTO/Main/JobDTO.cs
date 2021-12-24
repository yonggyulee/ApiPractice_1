using System;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Main;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Main
{
    public class JobDTO
    {
        public int Id { get; set; }                    // auto generation
        
        public int BatchJobId { get; set; }
        
        public string WorkerId { get; set; }
        
        public string Status { get; set; }
        
        public DateTime? ReadyStart { get; set; }
        
        public DateTime? ProgressStart { get; set; }
        
        public DateTime? ProgressEnd { get; set; }
    }
}