using AutoMapper;
using EducationalConsultAPI.Dtos;
using EducationalConsultAPI.Models;

namespace EducationalConsultAPI.AutoMapperProfiles {
    public class StudentToDtoProfile : Profile {
        public StudentToDtoProfile() {
            CreateMap<Student, StudentResponse>();
        }
    }
}
