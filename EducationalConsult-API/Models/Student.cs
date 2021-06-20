using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationalConsultAPI.Models {
    public class Student : User {
        public byte Age { get; set; }
        public string Gender { get; set; }
        public string Rank { get; set; }
        public string GuardianName { get; set; }
        public string GuardianEmail { get; set; }
        public string GuardianPhone { get; set; }
        public string Address { get; set; }
        public double CGPA { get; set; }

        public virtual Class Class { get; set; }
        public virtual IList<DailyReport> DailyReports { get; set; }

        [NotMapped]
        public override IList<Class> Classes { get; set; }
    }
}