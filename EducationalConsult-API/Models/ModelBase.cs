using System;
using System.ComponentModel.DataAnnotations;

namespace EducationalConsultAPI.Models {
    public class ModelBase {
        [Key]
        public Guid Id { get; set; }
    }
}
