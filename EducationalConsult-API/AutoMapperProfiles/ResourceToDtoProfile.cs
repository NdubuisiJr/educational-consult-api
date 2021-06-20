using AutoMapper;
using EducationalConsultAPI.Dtos;
using EducationalConsultAPI.Models;

namespace EducationalConsultAPI.AutoMapperProfiles {
    public class ResourceToDtoProfile  : Profile {
        public ResourceToDtoProfile() {
            CreateMap<Resource, ResourceResponse>();
        }
    }
}
