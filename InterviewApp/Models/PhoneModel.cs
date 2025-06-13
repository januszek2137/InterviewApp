using System.ComponentModel.DataAnnotations;

namespace InterviewApp.Models {
    public class PhoneModel {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        [StringLength(15, ErrorMessage = "The Name must be 15 characters or less")]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "The Number must have exactly 9 digits.")]
        public int Number { get; set; }
    }
}
