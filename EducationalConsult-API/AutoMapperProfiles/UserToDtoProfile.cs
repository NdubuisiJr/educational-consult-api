using AutoMapper;
using EducationalConsultAPI.Dtos;
using EducationalConsultAPI.Models;

namespace EducationalConsultAPI.AutoMapperProfiles {
    public class UserToDtoProfile : Profile {
        public UserToDtoProfile() {
            CreateMap<UserRegistration, User>();
            CreateMap<User, UserResponse>();
        }
    }
}
