using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeromylosSuites.Core;
using NeromylosSuites.Core.Filters;
using NeromylosSuites.DTO;
using NeromylosSuites.Exceptions;
using NeromylosSuites.Services;
using System.Security.Claims;

namespace NeromylosSuites.Controllers
{
    [ApiController]
    [Route("api/v1/bookings")]
    public class BookingsController : ControllerBase
    {
        private readonly IApplicationService _applicationService;

        public BookingsController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        /// <summary>
        /// Created a new booking.
        /// </summary>
        /// <param name="createBookingDTO">The booking details.</param>
        /// <returns>The created booking.</returns>
        /// <response code="200">Returns the created booking.</response>
        /// <response code="400">If the request body is invalid.</response>
        /// <response code="409">If one or more of the requested rooms are no longer available.</response>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BookingReadOnlyDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<BookingReadOnlyDTO>> CreateBookingAsync(
            [FromBody] CreateBookingDTO createBookingDTO)
        {
            var booking = await _applicationService.BookingService.CreateBookingAsync(createBookingDTO);
            return CreatedAtAction(nameof(GetBookingById), new { bookingId = booking.Id }, booking);
        }

        /// <summary>
        /// Deletes a booking (soft delete).
        /// </summary>
        /// <param name="bookingId">The booking ID to delete.</param>
        /// <response code="204">Booking deleted successfully.</response>
        /// <response code="401">If the request is not authenticated.</response>
        /// <response code="403">If the user is not an admin.</response>
        /// <response code="404">If no booking exists with the given id.</response>
        [HttpDelete("{bookingId:int}")]
        [Authorize(Roles = "ADMIN")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBooking(int bookingId)
        {
            await _applicationService.BookingService.DeleteBookingAsync(bookingId);
            return NoContent();
        }

        /// <summary>
        /// Get a booking by their id.
        /// </summary>
        /// <param name="bookingId">The bookingId to search for</param>
        /// <returns>the booking details.</returns>
        /// <response code="200">Returns the requested booking.</response>
        /// <response code="401">If the request is not authenticated.</response>
        /// <response code="403">If the user lacks permission to list bookings.</response>
        /// <response code="404">If no booking exists with the given id.</response>
        [HttpGet("{bookingId:int}")]
        [Authorize]
        [ProducesResponseType(typeof(BookingReadOnlyDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookingReadOnlyDTO>> GetBookingById(int bookingId)
        {
            var booking = await _applicationService.BookingService.GetBookingByIdAsync(bookingId);

            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var isOwnBooking = booking!.UserId == currentUserId;

            EnsureCanViewBookingCore(isOwnBooking);

            return Ok(booking);
        }

        /// <summary>
        /// Gets a booking by their userId.
        /// </summary>
        /// <param name="userId">The userId to search for </param>
        /// <returns>the booking details.</returns>
        /// <response code="200">Returns the requested booking.</response>
        /// <response code="401">If the request is not authenticated.</response>
        /// <response code="403">If the user lacks permission to list bookings.</response>
        /// <response code="404">If no booking exists with the given userId.</response>
        [HttpGet("bookings-by-userId/{userId}")]
        [Authorize]
        [ProducesResponseType(typeof(List<BookingReadOnlyDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<BookingReadOnlyDTO>>> GetBookingsByUserId(int userId)
        {
            EnsureCanViewBookingByUserId(userId);
            var booking = await _applicationService.BookingService.GetBookingsByUserIdAsync(userId);
            return Ok(booking);
        }

        /// <summary>
        /// Gets a booking by their visitorId.
        /// </summary>
        /// <param name="visitorId">The visitorId to search for </param>
        /// <returns>the booking details.</returns>
        /// <response code="200">Returns the requested booking.</response>
        /// <response code="401">If the request is not authenticated.</response>
        /// <response code="403">If the user lacks permission to list bookings.</response>
        /// <response code="404">If no booking exists with the given visitorId.</response>
        [HttpGet("bookings-by-visitorId/{visitorId}")]
        [Authorize(Roles = "ADMIN,RECEPTIONIST")]
        [ProducesResponseType(typeof(List<BookingReadOnlyDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<BookingReadOnlyDTO>>> GetBookingsByVisitorId(int visitorId)
        {
            var booking = await _applicationService.BookingService.GetBookingsByVisitorIdAsync(visitorId);
            return Ok(booking);
        }

        /// <summary>
        /// Gets a paginated list of bookings with optional filtering.
        /// </summary>
        /// <param name="pageNumber">The page number (1-based). Default is 1.</param>
        /// <param name="pageSize">The number of items per page. Default is 10.</param>
        /// <param name="filters">Optional filters for check-in, check-out and status.</param>
        /// <returns>A paginated list of bookings matching the filters.</returns>
        /// <response code="200">Returns the paginated booking list.</response>
        /// <response code="401">If the request is not authenticated.</response>
        /// <response code="403">If the user lacks permission to list bookings.</response>
        [HttpGet]
        [Authorize(Roles = "ADMIN,RECEPTIONIST")]
        [ProducesResponseType(typeof(PaginatedResult<BookingReadOnlyDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<PaginatedResult<BookingReadOnlyDTO>>> GetPaginatedBookingsFiltered(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] BookingFiltersDTO? filters = null)
        {
            var result = await _applicationService.BookingService
                .GetPaginatedBookingsFilteredAsync(pageNumber, pageSize, filters ?? new BookingFiltersDTO());

            return Ok(result);
        }

        private void EnsureCanViewBookingByUserId(int targetUserID)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var isOwnProfile = currentUserId == targetUserID;

            EnsureCanViewBookingCore(isOwnProfile);
        }

        private void EnsureCanViewBookingCore(bool isOwnProfile)
        {
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (isOwnProfile && currentUserRole == "GUEST") return;

            if (currentUserRole == "ADMIN" || currentUserRole == "RECEPTIONIST") return;

            throw new EntityForbiddenException("User",
                "You do not have permission to view this booking.");
        }
    }
}
