using System.ComponentModel.DataAnnotations;

namespace EducationalConsultAPI.Dtos {
    /// <summary>
    /// Model for changing password
    /// </summary>
    public class ResetPasswordDto {
        /// <summary>
        /// The old password
        /// </summary>
        [Required]
        public string OldPassword { get; set; }

        /// <summary>
        /// The new Password
        /// </summary>
        [Required]
        public string NewPassword { get; set; }
    }
}
