using System.Collections.Generic;

namespace EducationalConsultAPI.Models {
    public class User : ModelBase{
        public string Surname { get; set; }
        public string OtherNames { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ImageUrl { get; set; }
        public int VerificationCode { get; set; }
        public bool IsVerified { get; set; }
        
        public virtual IList<Class> Classes { get; set; }
    }
}
