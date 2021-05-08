using System.Collections.Generic;

namespace EducationalConsultAPI.Models {
    public class Class : ModelBase {
        public string Name { get; set; }

        public virtual User User { get; set; }
        public virtual School School { get; set; }
        public virtual IList<Resource> Resources { get; set; }
        public virtual IList<Student> Students { get; set; }
    }
}