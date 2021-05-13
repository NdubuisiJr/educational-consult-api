using System.ComponentModel.DataAnnotations;

namespace EducationalConsultAPI.Dtos {
    /// <summary>
    /// This is the form for creating a new school
    /// </summary>
    public class SchoolRegistration {
        /// <summary>
        /// The school's registered name
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// The school's address
        /// </summary>
        [Required]
        public string Address { get; set; }

        /// <summary>
        /// The school's official email
        /// </summary>
        [Required]
        public string OfficialEmail { get; set; }

        /// <summary>
        /// The school's official Phone
        /// </summary>
        /// 
        [Required]
        public string OfficialPhone { get; set; }

        /// <summary>
        /// The country where the head quartes resides
        /// </summary>
        /// 
        [Required]
        public string Country { get; set; }

        /// <summary>
        /// State where the head quarter resides
        /// </summary>
        /// 
        [Required]
        public string State { get; set; }

    }
}
