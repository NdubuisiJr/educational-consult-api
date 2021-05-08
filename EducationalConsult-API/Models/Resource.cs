namespace EducationalConsultAPI.Models {
    public class Resource : ModelBase {
        public string Link { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }

        public virtual Class Class { get; set; }
    }
}