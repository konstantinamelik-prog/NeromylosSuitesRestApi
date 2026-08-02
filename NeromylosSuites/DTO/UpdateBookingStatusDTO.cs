using System.ComponentModel.DataAnnotations;

namespace NeromylosSuites.DTO
{
    public record UpdateBookingStatusDTO
    {
        [Required(ErrorMessage = "The {0} field is required.")]
        public string? Status { get; set; }
    }
}