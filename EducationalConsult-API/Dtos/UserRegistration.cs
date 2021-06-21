using System.ComponentModel.DataAnnotations;

namespace EducationalConsultAPI.Dtos {
    /// <summary>
    /// The fields for creating an account
    /// </summary>
    public class UserRegistration {
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
        /// The user's email address
        /// </summary>
        /// 
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// The user's phone number.
        /// </summary>
        [Required]
        public string Phone { get; set; }

        /// <summary>
        /// The user's strong password
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// An optional redirect url
        /// </summary>
        public string Redirect { get; set; }

        /// <summary>
        /// An optional flag for students
        /// </summary>
        public bool IsStudent { get; set; }

    }
}
