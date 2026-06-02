using System.ComponentModel.DataAnnotations;

namespace NeromylosSuites.DTO
{
    public record CreateVisitorDTO
    {
        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Firstname must be between 2 and 50 characters.")]
        public string? Firstname { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Lastname must be between 2 and 50 characters.")]
        public string? Lastname { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Email must not exceed 100 characters.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "Phone number must be at least 10 characters and " +
            "not exceed 15 characters.")]
        public string? PhoneNumber { get; set; }

        [StringLength(2, MinimumLength = 2, ErrorMessage = "Country Code must be 2 characters, based on Country's 2-letter ISO Code standards")]
        public string? CountryCode { get; set; }
    }
}
