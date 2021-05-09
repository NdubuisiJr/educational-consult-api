using Microsoft.AspNetCore.Http;

namespace EducationalConsultAPI.Dtos {
    public class FileUpload {
        public IFormFile file { get; set; }
    }
}
