using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeromylosSuites.Core;
using NeromylosSuites.Core.Filters;
using NeromylosSuites.DTO;
using NeromylosSuites.Services;

namespace NeromylosSuites.Controllers
{
    [ApiController]
    [Route("api/v1/visitors")]
    public class VisitorsController : ControllerBase
    {
        private readonly IApplicationService _applicationService;

        public VisitorsController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        /// <summary>
        /// Creates a new visitor.
        /// </summary>
        /// <param name="createVisitorDTO">The visitor details.</param>
        /// <returns>The created visitor.</returns>
        /// <response code="201">Returns the created visitor.</response>
        /// <response code="400">If the request body is invalid.</response>
        /// <response code="409">If a visitor with the same email already exists.</response>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(VisitorReadOnlyDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<VisitorReadOnlyDTO>> CreateVisitor(
            [FromBody] CreateVisitorDTO createVisitorDTO)
        {
            var visitor = await _applicationService.VisitorService.CreateVisitorAsync(createVisitorDTO);
            return CreatedAtAction(nameof(GetVisitorById), new { visitorId = visitor.Id }, visitor);
        }

        /// <summary>
        /// Deletes a visitor (soft delete).
        /// </summary>
        /// <param name="visitorId">The visitor ID.</param>
        /// <response code="204">Visitor deleted successfully.</response>
        /// <response code="401">If the request is not authenticated.</response>
        /// <response code="403">If the user is not an admin.</response>
        /// <response code="404">If no visitor exists with the given id.</response>
        /// <response code="409">If the visitor has active or completed bookings.</response>
        [HttpDelete("{visitorId:int}")]
        [Authorize(Roles = "ADMIN")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteVisitor(int visitorId)
        {
            await _applicationService.VisitorService.DeleteVisitorAsync(visitorId);
            return NoContent();
        }

        /// <summary>
        /// Gets a visitor by their phoneNumber.
        /// </summary>
        /// <param name="phoneNumber">The phoneNumber to search for.</param>
        /// <returns>The visitor details.</returns>
        /// <response code="200">Returns the requested visitor.</response>
        /// <response code="401">If the request is not authenticated.</response>
        /// <response code="403">If the user lacks permission to list visitors.</response>
        /// <response code="404">If no visitor exists with the given phoneNumber.</response>
        [HttpGet("by-phoneNumber/{phoneNumber}")]
        [Authorize(Roles = "ADMIN,RECEPTIONIST")]
        [ProducesResponseType(typeof(VisitorReadOnlyDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VisitorReadOnlyDTO?>> GetVisitorByPhoneNumber(string phoneNumber)
        {
            var visitor = await _applicationService.VisitorService.GetVisitorByPhoneNumberAsync(phoneNumber);
            return Ok(visitor);
        }

        /// <summary>
        /// Gets a visitor by their visitorId.
        /// </summary>
        /// <param name="visitorId">The visitorId to search for.</param>
        /// <returns>The visitor details.</returns>
        /// <response code="200">Returns the requested visitor.</response>
        /// <response code="401">If the request is not authenticated.</response>
        /// <response code="403">If the user lacks permission to list visitors.</response>
        /// <response code="404">If no visitor exists with the given visitorId.</response>
        [HttpGet("{visitorId:int}")]
        [Authorize(Roles = "ADMIN,RECEPTIONIST")]
        [ProducesResponseType(typeof(VisitorReadOnlyDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VisitorReadOnlyDTO?>> GetVisitorById(int visitorId)
        {
            var visitor = await _applicationService.VisitorService.GetVisitorByIdAsync(visitorId);
            return Ok(visitor);
        }

        /// <summary>
        /// Gets a visitor's bookings by their visitorId.
        /// </summary>
        /// <param name="visitorId">The visitorId to search for.</param>
        /// <returns>A list of visitor's bookings.</returns>
        /// <response code="200">Returns the requested bookings.</response>
        /// <response code="401">If the request is not authenticated.</response>
        /// <response code="404">If no visitor exists with the given visitorId.</response>
        [HttpGet("bookings-by-visitorId/{visitorId}")]
        [Authorize]
        [ProducesResponseType(typeof(List<BookingReadOnlyDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<BookingReadOnlyDTO>>> GetVisitorBookings(int visitorId)
        {
            var bookings = await _applicationService.VisitorService.GetVisitorBookingsAsync(visitorId);
            return Ok(bookings);
        }

        /// <summary>
        /// Gets visitors by a Country Code.
        /// </summary>
        /// <param name="countryCode">The countryCode to search for.</param>
        /// <returns>A list of visitors.</returns>
        /// <response code="200">Returns the requested list of visitors.</response>
        /// <response code="401">If the request is not authenticated.</response>
        /// <response code="403">If the user lacks permission to list visitors.</response>
        [HttpGet("visitors-by-countryCode/{countryCode}")]
        [Authorize(Roles = "ADMIN,RECEPTIONIST")]
        [ProducesResponseType(typeof(List<VisitorReadOnlyDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<List<VisitorReadOnlyDTO>>> GetVisitorsByCountryCode(string countryCode)
        {
            var visitors = await _applicationService.VisitorService.GetVisitorsByCountryCodeAsync(countryCode);
            return Ok(visitors);
        }

        /// <summary>
        /// Gets a paginated list of visitors with optional filtering.
        /// </summary>
        /// <param name="pageNumber">The page number (1-based). Default is 1.</param>
        /// <param name="pageSize">The number of items per page. Default is 10.</param>
        /// <param name="filters">Optional filters for email, lastname and country code.</param>
        /// <returns>A paginated list of visitors matching the filters.</returns>
        /// <response code="200">Returns the paginated visitor list.</response>
        /// <response code="401">If the request is not authenticated.</response>
        /// <response code="403">If the user lacks permission to list visitors.</response>
        [HttpGet]
        [Authorize(Roles = "ADMIN,RECEPTIONIST")]
        [ProducesResponseType(typeof(PaginatedResult<VisitorReadOnlyDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<PaginatedResult<VisitorReadOnlyDTO>>> GetPaginatedVisitorsFilteredAsync(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] VisitorFiltersDTO? filters = null)
        {
            var result = await _applicationService.VisitorService
                .GetPaginatedVisitorsFilteredAsync(pageNumber, pageSize, filters ?? new VisitorFiltersDTO());

            return Ok(result);
        }
    }
}
