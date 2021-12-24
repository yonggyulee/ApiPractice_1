﻿using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Main;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Main
{
    public class DatasetDTO
    {
        public int Id { get; set; }                
        
        public string Name { get; set; }
        
        public string VolumeId { get; set; }
        
        public string Uri { get; set; }
        
        public string? Descriptions { get; set; }
    }
}