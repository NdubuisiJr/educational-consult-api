using AutoMapper;
using EducationalConsultAPI.Dtos;
using EducationalConsultAPI.Models;

namespace EducationalConsultAPI.AutoMapperProfiles {
    public class ActivityToDtoProfile : Profile {
        public ActivityToDtoProfile() {
            CreateMap<Activity, ActivityResponse>();
        }
    }
}
