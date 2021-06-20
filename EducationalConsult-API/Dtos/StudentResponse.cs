
using System;
using System.Collections.Generic;

namespace EducationalConsultAPI.Dtos {
    public class StudentResponse {
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
        /// The student's age
        /// </summary>
        public byte Age { get; set; }

        /// <summary>
        /// The student's gender
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// The student's current rank
        /// </summary>
        public string Rank { get; set; }

        /// <summary>
        /// The guadian's name
        /// </summary>
        public string GuardianName { get; set; }

        /// <summary>
        /// The guadian's email
        /// </summary>
        public string GuardianEmail { get; set; }

        /// <summary>
        /// The guadian's phone
        /// </summary>
        public string GuardianPhone { get; set; }

        /// <summary>
        /// The student's address
        /// </summary>
        public string Address { get; set; }


        public virtual IList<DailyReportResponse> DailyReports { get; set; }
    }
}
