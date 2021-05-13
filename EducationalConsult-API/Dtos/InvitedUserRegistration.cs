using System.ComponentModel.DataAnnotations;

namespace EducationalConsultAPI.Dtos {
    /// <summary>
    /// The structure for adding a teacher, admin or student
    /// </summary>
    public class InvitedUserRegistration {
        /// <summary>
        /// The invited email
        /// </summary>
        /// 
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// The invitation role
        /// </summary>
        [Required]
        public string Role { get; set; }
    }
}
