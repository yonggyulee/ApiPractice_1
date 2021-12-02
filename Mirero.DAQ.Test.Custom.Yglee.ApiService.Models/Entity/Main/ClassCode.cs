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
        [Column("id")]
        public int ID { get; set; }                    // auto generation
        
        [Column("class_code")]
        public string Class_Code { get; set; }
        
        [Column("name")]
        public string Name { get; set; }
        
        [Column("info")]
        public string Info { get; set; }                // json
        
        [Column("class_code_set_id")]
        public string ClassCodeSetID { get; set; }
        
        [ForeignKey("ClassCodeSetID")]
        public ClassCodeSet? ClassCodeSet { get; set; } = null;
    }
}