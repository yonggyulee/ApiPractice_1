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
        [Column("user_id")]
        public string UserID { get; set; }
        
        [Key]
        [Column("auth_id")]
        public string AuthID { get; set; }
        
        [ForeignKey("UserID")]
        public User? User { get; set; } = null;
        
        [ForeignKey("AuthID")]
        public Auth? Auth { get; set; } = null;
    }
}