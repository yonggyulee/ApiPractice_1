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
        public string Id { get; set; }
        
        public string Uri { get; set; }
        
        public string Type { get; set; }
        
        public int Usage { get; set; }      // KB
        
        public int Capacity { get; set; }   // KB

        public ICollection<Dataset>? Datasets { get; set; } = new List<Dataset>();

        public ICollection<Artifact>? Artifacts { get; set; } = new List<Artifact>();
    }
}