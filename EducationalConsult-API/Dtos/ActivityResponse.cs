namespace EducationalConsultAPI.Dtos {
    public class ActivityResponse {
        /// <summary>
        /// The activity Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The score
        /// </summary>
        public double Score { get; set; }

        /// <summary>
        /// The overall score
        /// </summary>
        public double OverallScore { get; set; }

        /// <summary>
        /// The grade point Average
        /// </summary>
        public double Average { get; set; }

        /// <summary>
        /// The grade
        /// </summary>
        public string Grade { get; set; }

        /// <summary>
        /// The grade point
        /// </summary>
        public double GradePoint { get; set; }

        /// <summary>
        /// The grade remark
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// The category of this activity
        /// </summary>
        public string Category { get; set; }

    }
}
