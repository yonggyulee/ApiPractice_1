using System;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Main;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Main
{
    public class ClassCodeDTO
    {
        public int Id { get; set; }                    
        
        public string Class_Code { get; set; }
        
        public string Name { get; set; }
        
        public string Info { get; set; }                
        
        public string ClassCodeSetId { get; set; }
    }
}