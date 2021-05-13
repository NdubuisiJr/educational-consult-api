using System;
using System.Collections.Generic;

namespace EducationalConsultAPI.Dtos {
    public class SchoolResponse {

        /// <summary>
        /// School Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The school's registered name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The school's address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// The school's official email
        /// </summary>
        public string OfficialEmail { get; set; }

        /// <summary>
        /// The school's official Phone
        /// </summary>
        public string OfficialPhone { get; set; }

        /// <summary>
        /// The country where the head quartes resides
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// State where the head quarter resides
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Logo link
        /// </summary>
        public string LogoUrl { get; set; }

        /// <summary>
        /// All the classes in the school
        /// </summary>
        public virtual IList<ClassResponse> Classes { get; set; }

        /// <summary>
        /// All the groups in the school
        /// </summary>
        public virtual IList<GroupResponse> Groups { get; set; }
    }
}
