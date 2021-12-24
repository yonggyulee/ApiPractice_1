using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Main;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Main
{
    [Table("auth")]
    public class Auth
    {
        [Key]
        public string Id { get; set; }
        
        public string Name { get; set; }
        
        public string? Descriptions { get; set; } = null;
        
        public ICollection<UserAuthMap>? UserAuthMaps { get; set; } = new List<UserAuthMap>();
    }
}