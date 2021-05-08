using EducationalConsultAPI.Dtos;

namespace EducationalConsultAPI.Interfaces {
    public interface ISecurityService {
        string GenerateJwtToken(UserDto user);
    }
}
