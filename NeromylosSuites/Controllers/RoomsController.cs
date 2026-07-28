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
    [Route("api/v1/rooms")]
    public class RoomsController : ControllerBase
    {
        private readonly IApplicationService _applicationService;

        public RoomsController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        /// <summary>
        /// Gets the available rooms for a specific date range.
        /// </summary>
        /// <param name="checkIn">The date of check-in (example, 2026-07-28)</param>
        /// <param name="checkOut">The date of check-out (example, 2026-07-28) to search for</param>
        /// <returns>the list of rooms.</returns>
        /// <response code="200">Returns the requested availability.</response>
        /// <response code="400">If the request body is invalid.</response>
        [HttpGet("available")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<RoomReadOnlyDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<RoomReadOnlyDTO>>> GetAvailableRoomsByDateRange(
            [FromQuery] DateTime checkIn,
            [FromQuery] DateTime checkOut)
        {
            var availableRooms = await _applicationService.RoomService.GetAvailableRoomsByDateRangeAsync(checkIn, checkOut);

            return Ok(availableRooms);
        }

        /// <summary>
        /// Gets the bookings of a specific roomName.
        /// </summary>
        /// <param name="roomName">The roomName to search for</param>
        /// <returns>the list of bookings.</returns>
        /// <response code="200">Returns the requested list of bookings.</response>
        /// <response code="401">If the request is not authenticated.</response>
        /// <response code="403">If the user lacks permission to list bookings.</response>
        /// <response code="404">If no booking exists with the given roomName.</response>
        [HttpGet("room-bookings/{roomName}")]
        [Authorize(Roles = "ADMIN,RECEPTIONIST")]
        [ProducesResponseType(typeof(List<BookingReadOnlyDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<BookingReadOnlyDTO>>> GetRoomBookings(string roomName)
        {
            var bookings = await _applicationService.RoomService.GetRoomBookingsAsync(roomName);

            return Ok(bookings);
        }

        /// <summary>
        /// Gets a paginated list of rooms with optional filtering.
        /// </summary>
        /// <param name="pageNumber">The page number (1-based). Default is 1.</param>
        /// <param name="pageSize">The number of items per page. Default is 10.</param>
        /// <param name="filters">Optional filters for check-in, check-out and status.</param>
        /// <returns>A paginated list of rooms matching the filters.</returns>
        /// <response code="200">Returns the paginated booking list.</response>
        /// <response code="401">If the request is not authenticated.</response>
        /// <response code="403">If the user lacks permission to list bookings.</response>
        [HttpGet]
        [Authorize(Roles = "ADMIN,RECEPTIONIST")]
        [ProducesResponseType(typeof(PaginatedResult<RoomReadOnlyDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<PaginatedResult<RoomReadOnlyDTO>>> GetPaginatedRoomsFiltered(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] RoomFiltersDTO? filters = null)
        {
            var result = await _applicationService.RoomService
                .GetPaginatedRoomsFilteredAsync(pageNumber, pageSize, filters ?? new RoomFiltersDTO());

            return Ok(result);
        }
    }
}
