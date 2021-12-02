using System;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Main;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Main
{
    public class JobDTO
    {
        public int ID { get; set; }                    // auto generation
        
        public int BatchJobID { get; set; }
        
        public string WorkerID { get; set; }
        
        public string Status { get; set; }
        
        public DateTime? ReadyStart { get; set; }
        
        public DateTime? ProgressStart { get; set; }
        
        public DateTime? ProgressEnd { get; set; }
    }
}