using System.Collections.Generic;

namespace EducationalConsultAPI.Models {
    public class School : ModelBase{
        public string Name { get; set; }
        public string Address { get; set; }
        public string OfficialEmail { get; set; }
        public string OfficialPhone { get; set; }
        public string LogoUrl { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public double GreadeScale { get; set; }

        public virtual IList<Class> Classes { get; set; }
        public virtual IList<Group> Groups { get; set; }
    }
}