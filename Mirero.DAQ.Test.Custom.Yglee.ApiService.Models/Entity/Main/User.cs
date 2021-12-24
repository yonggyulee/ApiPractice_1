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
        public string Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Password { get; set; }
        
        public string? Email { get; set; } = null;
        
        public string? Descriptions { get; set; } = null;
        
        public ICollection<UserAuthMap>? UserAuthMaps { get; set; } = new List<UserAuthMap>();
    }
}