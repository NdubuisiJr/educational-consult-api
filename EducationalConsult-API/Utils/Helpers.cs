using System.IO;

namespace EducationalConsultAPI.Utils {
    public static class Helpers {
        public static string GetFilePath(string imageUrl) {
            var fileName = imageUrl.Substring(imageUrl.LastIndexOf("/") + 1);
            return Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileName);
        }

        
    }
}
