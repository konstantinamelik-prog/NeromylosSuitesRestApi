using System.ComponentModel.DataAnnotations;

namespace NeromylosSuites.DTO
{
    public class AvailabilitySearchDTO
    {
        [Required(ErrorMessage = "The {0} field is required.")]
        public DateTime? CheckIn { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        public DateTime? CheckOut { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        public int? NumberOfGuests { get; set; }
    }
}
