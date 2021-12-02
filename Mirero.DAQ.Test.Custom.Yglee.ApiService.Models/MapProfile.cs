using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Main;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Storage;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Main;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Models
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            // Main
            CreateMap<Artifact, ArtifactDTO>().ReverseMap();
            CreateMap<Auth, AuthDTO>().ReverseMap();
            CreateMap<BatchJob, BatchJobDTO>().ReverseMap();
            CreateMap<ClassCode, ClassCodeDTO>().ReverseMap();
            CreateMap<ClassCodeSet, ClassCodeSetDTO>().ReverseMap();
            CreateMap<Dataset, DatasetDTO>().ReverseMap();
            CreateMap<Job, JobDTO>().ReverseMap();
            CreateMap<UserAuthMap, UserAuthMapDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();

            // Storage
            CreateMap<Sample, SampleDTO>().ReverseMap();
            CreateMap<Image, ImageDTO>().ReverseMap();
            CreateMap<LabelSet, LabelSetDTO>().ReverseMap();
            CreateMap<ClassificationLabel, ClassificationLabelDTO>().ReverseMap();
            CreateMap<ObjectDetectionLabel, ObjectDetectionLabelDTO>().ReverseMap();
            CreateMap<SegmentationLabel, SegmentationLabelDTO>().ReverseMap();
        }
    }
}
