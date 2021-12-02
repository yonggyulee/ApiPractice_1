using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Main
{
    [Table("volume")]
    public class Volume
    {
        [Key]
        [Column("id")]
        public string ID { get; set; }
        
        [Column("uri")]
        public string URI { get; set; }
        
        [Column("type")]
        public string Type { get; set; }
        
        [Column("usage")]
        public int Usage { get; set; }      // KB
        
        [Column("capacity")]
        public int Capacity { get; set; }   // KB

        public ICollection<Dataset>? Datasets { get; set; } = new List<Dataset>();

        public ICollection<Artifact>? Artifacts { get; set; } = new List<Artifact>();
    }
}