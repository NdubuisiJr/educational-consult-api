using AutoMapper;
using EducationalConsultAPI.Dtos;
using EducationalConsultAPI.Models;

namespace EducationalConsultAPI.AutoMapperProfiles {
    public class InvitedUserDtoProfile : Profile {
        public InvitedUserDtoProfile() {
            CreateMap<InvitedUser, InvitedUserResponse>();
            CreateMap<InvitedUserRegistration, InvitedUser>();
        }
    }
}
