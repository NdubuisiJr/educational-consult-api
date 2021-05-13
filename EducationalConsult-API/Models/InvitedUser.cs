namespace EducationalConsultAPI.Models {
    public class InvitedUser : ModelBase {
        public string Email { get; set; }
        public string Role { get; set; }

        public virtual Group Group { get; set; }

    }
}
