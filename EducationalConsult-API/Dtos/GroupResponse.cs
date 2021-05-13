using System;
using System.Collections.Generic;

namespace EducationalConsultAPI.Dtos {
    /// <summary>
    /// The structure of group
    /// </summary>
    public class GroupResponse {
        /// <summary>
        /// The Id of the group
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The Role for this group 
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// All the invited users, yet to accept
        /// </summary>
        public IList<InvitedUserResponse> InvitedUsers { get; set; }

        /// <summary>
        /// All the current users on this role/group
        /// </summary>
        public List<UserResponse> Users { get; set; } = new List<UserResponse>();
    }
}
