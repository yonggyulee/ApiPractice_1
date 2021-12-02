using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Main;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Main
{
    public class ArtifactDTO
    {
        public int ID { get; set; }                
        
        public int JobID { get; set; }
        
        public string Name { get; set; }
        
        public string VolumeID { get; set; }
        
        public string URI { get; set; }

        public string? Descriptions { get; set; }
    }
}