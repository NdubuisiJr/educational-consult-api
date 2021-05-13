using System;

namespace EducationalConsultAPI.Dtos {
    public class InvitedUserResponse {
        /// <summary>
        /// The documented Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The invited user's email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The invited user's role
        /// </summary>
        public string Role { get; set; }
    }
}