using System;
using System.Collections.Generic;

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

        /// <summary>
        /// The Teacher for this class
        /// </summary>
        public UserClassResponse User { get; set; }

        /// <summary>
        /// The resources for this class
        /// </summary>
        public IList<ResourceResponse> Resources { get; set; }

        /// <summary>
        /// The Students in this class
        /// </summary>
        public virtual IList<StudentResponse> Students { get; set; }
    }

    public class ClassResponseForStudent {
        /// <summary>
        /// The Id of the class
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the group
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The Teacher for this class
        /// </summary>
        public UserClassResponse User { get; set; }

        /// <summary>
        /// The resources for this class
        /// </summary>
        public IList<ResourceResponse> Resources { get; set; }
    }
}
