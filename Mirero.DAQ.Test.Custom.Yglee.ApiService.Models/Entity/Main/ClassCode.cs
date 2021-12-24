using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Main;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Main
{
    [Table("class_code")]
    public class ClassCode
    {
        [Key]
        public int Id { get; set; }                    // auto generation
        
        public string Class_Code { get; set; }
        
        public string Name { get; set; }
        
        public string Info { get; set; }                // json
        
        public string ClassCodeSetId { get; set; }
        
        [ForeignKey("ClassCodeSetId")]
        public ClassCodeSet? ClassCodeSet { get; set; } = null;
    }
}