using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Main;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Main
{
    [Table("class_code_set")]
    public class ClassCodeSet
    {
        [Key]
        [Column("id")]
        public string ID { get; set; }

        [Column("task_type")]
        public string TaskType { get; set; }
        
        [Column("create_at")]
        public DateTime? CreatedAt { get; set; } = null;
        
        // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("update_at")]
        public DateTime? UpdatedAt { get; set; } = null;

        public ICollection<ClassCode>? ClassCodes { get; set; } = new List<ClassCode>();
    }
}