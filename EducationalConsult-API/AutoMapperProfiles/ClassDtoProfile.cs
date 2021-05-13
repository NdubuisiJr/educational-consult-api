using AutoMapper;
using EducationalConsultAPI.Dtos;
using EducationalConsultAPI.Models;

namespace EducationalConsultAPI.AutoMapperProfiles {
    public class ClassDtoProfile : Profile {
        public ClassDtoProfile() {
            CreateMap<Class, ClassResponse>();
        }
    }
}
