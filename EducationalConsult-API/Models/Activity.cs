namespace EducationalConsultAPI.Models {
    public class Activity : ModelBase {
        public string Name { get; set; }
        public double Score { get; set; }
        public double OverallScore { get; set; }
        public double Average { get; set; }
        public string Grade { get; set; }
        public double GradePoint { get; set; }
        public string Remark { get; set; }
        public string Category { get; set; }
        public double CGPA { get; set; }
        public string Rank { get; set; }

        public virtual DailyReport DailyReport { get; set; }
    }
}