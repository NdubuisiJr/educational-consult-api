using System.ComponentModel.DataAnnotations;

namespace EducationalConsultAPI.Dtos {
    /// <summary>
    /// Login structure
    /// </summary>
    public class UserLogin {
        /// <summary>
        /// User's email required
        /// </summary>
        [Required]
        public string Email { get; set; }
        
        /// <summary>
        /// User's Password. Of course it is required
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
