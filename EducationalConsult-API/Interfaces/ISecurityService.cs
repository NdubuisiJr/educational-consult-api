using EducationalConsultAPI.Models;

namespace EducationalConsultAPI.Interfaces {
    public interface ISecurityService {
        string GenerateJwtToken(User user);
        (string, string) HashPassword(string password, int saltSize = 10);
        bool VerifyPassword(string password, string hashedPassword, string saltString);
    }
}
