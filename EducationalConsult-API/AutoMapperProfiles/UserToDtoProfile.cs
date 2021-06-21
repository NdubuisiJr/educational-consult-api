using AutoMapper;
using EducationalConsultAPI.Dtos;
using EducationalConsultAPI.Models;

namespace EducationalConsultAPI.AutoMapperProfiles {
    public class UserToDtoProfile : Profile {
        public UserToDtoProfile() {
            CreateMap<UserRegistration, User>();
            CreateMap<UserRegistration, Student>();
            CreateMap<User, UserResponse>();
            CreateMap<User, UserClassResponse>();
        }
    }
}
