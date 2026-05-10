using System.ComponentModel.DataAnnotations;

namespace NeromylosSuites.DTO
{
    public class CreateBookingDTO
    {
        [Required(ErrorMessage = "The {0} field is required.")]
        public DateTime? CheckIn { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        public DateTime? CheckOut { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        public int? NumberOfGuests { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        public decimal? TotalPrice { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        public string? Status { get; set; } = "PENDING";

        [Required(ErrorMessage = "The {0} field is required.")]
        public int? UserId { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        public int? VisitorId { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        public List<int>? RoomIds { get; set; } 
    }
}
