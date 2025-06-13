using System.ComponentModel.DataAnnotations;

namespace InterviewApp.Models {
    public class PhoneModel {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Number { get; set; }
    }
}
