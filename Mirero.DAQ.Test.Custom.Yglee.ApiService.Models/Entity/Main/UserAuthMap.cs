using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Main;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Main
{
    [Table("user_auth_map")]
    public class UserAuthMap
    {
        [Key]
        public string UserId { get; set; }
        
        [Key]
        public string AuthId { get; set; }
        
        [ForeignKey("UserId")]
        public User? User { get; set; } = null;
        
        [ForeignKey("AuthId")]
        public Auth? Auth { get; set; } = null;
    }
}