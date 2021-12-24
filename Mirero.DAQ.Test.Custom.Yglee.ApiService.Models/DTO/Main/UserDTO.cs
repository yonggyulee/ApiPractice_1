using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Main;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Main
{
    public class UserDTO
    {
        public string Id { get; set; }
        
        public string Name { get; set; }
        
        public string Password { get; set; }
        
        public string? Email { get; set; }
        
        public string? Descriptions { get; set; }
    }
}