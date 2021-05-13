using EducationalConsultAPI.Models;
using System;
using System.Collections.Generic;

namespace EducationalConsultAPI.Dtos {
    /// <summary>
    /// The returned user object
    /// </summary>
    public class UserResponse {
        /// <summary>
        /// The user's unique Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The user's Surname
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// The user's other names
        /// </summary>
        public string OtherNames { get; set; }

        /// <summary>
        /// The user's email address
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The user's phone number.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// The user's profile picture
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Authentication token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Collection of classes created by this user
        /// </summary>
        public IList<Class> Classes { get; set; }
    }
}
