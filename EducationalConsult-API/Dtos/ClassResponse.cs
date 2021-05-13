using System;

namespace EducationalConsultAPI.Dtos {
    /// <summary>
    /// The structure of a class
    /// </summary>
    public class ClassResponse {
        /// <summary>
        /// The Id of the class
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the group
        /// </summary>
        public string Name { get; set; }
    }
}
