namespace EducationalConsultAPI.Models {
    public class Group : ModelBase{
        public string Role { get; set; }

        public virtual School School { get; set; }
    }
}
