using Microsoft.AspNetCore.Http;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Storage
{
    public class ImageDTO
    {
        public string ID { get; set; }
        
        public string SampleID { get; set; }
        
        public string ImageCode { get; set; }

        public string OriginalFilename { get; set; }

        public int Width { get; set; }
        
        public int Height { get; set; }
        
        public int Channel { get; set; }
        
        public string Dtype { get; set; }
        
        public string Path { get; set; }
    }
}