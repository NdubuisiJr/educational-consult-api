using AutoMapper;
using EducationalConsultAPI.Dtos;
using EducationalConsultAPI.Models;

namespace EducationalConsultAPI.AutoMapperProfiles {
    public class SchoolToDtoProfile : Profile {
        public SchoolToDtoProfile() {
            CreateMap<School, SchoolResponse>()
                    .ForMember(x => x.Classes, x => x.MapFrom(x => x.Classes))
                    .ForMember(x => x.Groups, x => x.MapFrom(x => x.Groups));
            CreateMap<SchoolRegistration, School>();
        }
    }
}
