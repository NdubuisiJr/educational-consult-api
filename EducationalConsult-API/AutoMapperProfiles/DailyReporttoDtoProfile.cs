using AutoMapper;
using EducationalConsultAPI.Dtos;
using EducationalConsultAPI.Models;

namespace EducationalConsultAPI.AutoMapperProfiles {
    public class DailyReporttoDtoProfile : Profile {
        public DailyReporttoDtoProfile() {
            CreateMap<DailyReport, DailyReportResponse>();
        }
    }
}
