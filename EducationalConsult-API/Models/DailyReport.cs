using System;
using System.Collections.Generic;

namespace EducationalConsultAPI.Models {
    public class DailyReport : ModelBase {
        public string Term { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual DateTime TermStartDate { get; set; }
        public virtual DateTime TermEndDate { get; set; }
        
        public virtual Student Student { get; set; }
        public virtual IList<Activity> Activities { get; set; }
    }
}