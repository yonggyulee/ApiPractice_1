using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Main;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Main
{
    [Table("user")]
    public class User
    {
        [Key]
        [Column("id")]
        public string ID { get; set; }
        
        [Required]
        [Column("name")]
        public string Name { get; set; }
        
        [Required]
        [Column("password")]
        public string Password { get; set; }
        
        [Column("email")]
        public string? Email { get; set; } = null;
        
        [Column("descriptions")]
        public string? Descriptions { get; set; } = null;
        
        public ICollection<UserAuthMap>? UserAuthMaps { get; set; } = new List<UserAuthMap>();
    }
}