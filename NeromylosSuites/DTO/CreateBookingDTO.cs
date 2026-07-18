using System.ComponentModel.DataAnnotations;

namespace NeromylosSuites.DTO
{
    public record CreateBookingDTO
    {
        [Required(ErrorMessage = "The {0} field is required.")]
        public DateTime? CheckIn { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        public DateTime? CheckOut { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        public int? NumberOfGuests { get; set; }

        [StringLength(255, ErrorMessage = "Special Requests must be maximum 255 characters.")]
        public string? SpecialRequests { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(50, MinimumLength = 2)]
        public string? Firstname { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(50, MinimumLength = 2)]
        public string? Lastname { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(15, MinimumLength = 10)]
        public string? PhoneNumber { get; set; }

        [StringLength(2, MinimumLength = 2)]
        public string? CountryCode { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        public List<int>? RoomIds { get; set; } 
    }
}
