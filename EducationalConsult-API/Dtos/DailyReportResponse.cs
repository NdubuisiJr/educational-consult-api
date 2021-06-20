using System;
using System.Collections.Generic;

namespace EducationalConsultAPI.Dtos {
    public class DailyReportResponse {
        /// <summary>
        /// The current term for this report
        /// </summary>
        public string Term { get; set; }

        /// <summary>
        /// The date of filling
        /// </summary>
        public virtual DateTime Date { get; set; }

        /// <summary>
        /// The term start date
        /// </summary>
        public virtual DateTime TermStartDate { get; set; }

        /// <summary>
        /// The term end date
        /// </summary>
        public virtual DateTime TermEndDate { get; set; }

        /// <summary>
        /// The activity for this day
        /// </summary>
        public virtual IList<ActivityResponse> Activities { get; set; }
    }
}
