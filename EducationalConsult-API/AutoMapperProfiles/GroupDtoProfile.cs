using AutoMapper;
using EducationalConsultAPI.Dtos;
using EducationalConsultAPI.Models;

namespace EducationalConsultAPI.AutoMapperProfiles {
    public class GroupDtoProfile : Profile{
        public GroupDtoProfile() {
            CreateMap<Group, GroupResponse>()
                .ForMember(x => x.InvitedUsers, x => x.MapFrom(x => x.InvitedUsers));
        }
    }
}
