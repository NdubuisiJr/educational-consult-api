using System.ComponentModel.DataAnnotations;

namespace EducationalConsultAPI.Dtos {
    /// <summary>
    /// model for upating user details
    /// </summary>
    public class UserUpdateDto {
        /// <summary>
        /// The user's Surname
        /// </summary>
        /// 
        [Required]
        public string Surname { get; set; }

        /// <summary>
        /// The user's other names
        /// </summary>
        /// 
        [Required]
        public string OtherNames { get; set; }

        /// <summary>
        /// The user's phone number.
        /// </summary>
        [Required]
        public string Phone { get; set; }
    }
}
