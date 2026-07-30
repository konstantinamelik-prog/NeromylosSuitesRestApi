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
    [Route("api/v1/members")]
    public class MembersController : ControllerBase
    {
        private readonly IApplicationService _applicationService;

        public MembersController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        /// <summary>
        /// Gets a member by their phoneNumber.
        /// </summary>
        /// <param name="phoneNumber">The phoneNumber to search for.</param>
        /// <returns>The member details.</returns>
        /// <response code="200">Returns the requested member.</response>
        /// <response code="401">If the request is not authenticated.</response>
        /// <response code="403">If the user lacks permission to list members.</response>
        /// <response code="404">If no member exists with the given phoneNumber.</response>
        [HttpGet("by-phoneNumber/{phoneNumber}")]
        [Authorize(Roles = "ADMIN,RECEPTIONIST")]
        [ProducesResponseType(typeof(MemberReadOnlyDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MemberReadOnlyDTO>> GetMemberByPhoneNumber(string phoneNumber)
        {
            var member = await _applicationService.MemberService.GetMemberByPhoneNumberAsync(phoneNumber);
            return Ok(member);
        }

        /// <summary>
        /// Gets a member by their username.
        /// </summary>
        /// <param name="username">The username to search for.</param>
        /// <returns>The member details.</returns>
        /// <response code="200">Returns the requested member.</response>
        /// <response code="401">If the request is not authenticated.</response>
        /// <response code="403">If the user lacks permission to list members.</response>
        /// <response code="404">If no member exists with the given username.</response>
        [HttpGet("by-username/{username}")]
        [Authorize]
        [ProducesResponseType(typeof(MemberReadOnlyDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MemberReadOnlyDTO>> GetUserMemberByUsername(string username)
        {
            EnsureCanViewUserMemberByUsername(username);
            var member = await _applicationService.MemberService.GetUserMemberByUsernameAsync(username);

            return Ok(member);
        }

        /// <summary>
        /// Gets a user with member by their userId.
        /// </summary>
        /// <param name="userId">The userId to search for.</param>
        /// <returns>The user with member details.</returns>
        /// <response code="200">Returns the requested user.</response>
        /// <response code="401">If the request is not authenticated.</response>
        /// <response code="403">If the user lacks permission to list users or members.</response>
        /// <response code="404">If no user exists with the given userId.</response>
        [HttpGet("user-member-by-userId/{userId}")]
        [Authorize]
        [ProducesResponseType(typeof(UserReadOnlyDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserReadOnlyDTO>> GetUserWithMemberById(int userId)
        {
            EnsureCanViewUserMemberByUserId(userId);
            var member = await _applicationService.MemberService.GetUserWithMemberByIdAsync(userId);

            return Ok(member);
        }

        /// <summary>
        /// Gets a member's bookings by their userId.
        /// </summary>
        /// <param name="userId">The userId to search for.</param>
        /// <returns>A list of member's bookings.</returns>
        /// <response code="200">Returns the requested bookings.</response>
        /// <response code="401">If the request is not authenticated.</response>
        /// <response code="404">If no member exists with the given userId.</response>
        [HttpGet("bookings-by-userId/{userId}")]
        [Authorize]
        [ProducesResponseType(typeof(List<BookingReadOnlyDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<BookingReadOnlyDTO>>> GetMemberBookings(int userId)
        {
            EnsureCanViewUserMemberByUserId(userId);
            var bookings = await _applicationService.MemberService.GetMemberBookingsAsync(userId);

            return Ok(bookings);
        }

        /// <summary>
        /// Gets members by a Country Code.
        /// </summary>
        /// <param name="countryCode">The countryCode to search for.</param>
        /// <returns>A list of members.</returns>
        /// <response code="200">Returns the requested list of members.</response>
        /// <response code="401">If the request is not authenticated.</response>
        /// <response code="403">If the user lacks permission to list members.</response>
        [HttpGet("members-by-countryCode/{countryCode}")]
        [Authorize(Roles = "ADMIN,RECEPTIONIST")]
        [ProducesResponseType(typeof(List<MemberReadOnlyDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<List<MemberReadOnlyDTO>>> GetMembersByCountryCodeAsync(string countryCode)
        {
            var members = await _applicationService.MemberService.GetMembersByCountryCodeAsync(countryCode);
            return Ok(members);
        }

        /// <summary>
        /// Gets a paginated list of members with optional filtering.
        /// </summary>
        /// <param name="pageNumber">The page number (1-based). Default is 1.</param>
        /// <param name="pageSize">The number of items per page. Default is 10.</param>
        /// <param name="filters">Optional filters for country code.</param>
        /// <returns>A paginated list of members matching the filters.</returns>
        /// <response code="200">Returns the paginated member list.</response>
        /// <response code="401">If the request is not authenticated.</response>
        /// <response code="403">If the user lacks permission to list members.</response>
        [HttpGet]
        [Authorize(Roles = "ADMIN,RECEPTIONIST")]
        [ProducesResponseType(typeof(PaginatedResult<UserReadOnlyDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<PaginatedResult<UserReadOnlyDTO>>> GetPaginatedMembersFiltered(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] MemberFiltersDTO? filters = null)
        {
            var result = await _applicationService.MemberService
                .GetPaginatedMembersFilteredAsync(pageNumber, pageSize, filters ?? new MemberFiltersDTO());

            return Ok(result);
        }

        private void EnsureCanViewUserMemberByUsername(string username)
        {
            var currentUsername = User.FindFirst(ClaimTypes.Name)?.Value;
            var isOwnProfile = string.Equals(currentUsername, username, StringComparison.OrdinalIgnoreCase);

            EnsureCanViewUserMemberCore(isOwnProfile);
        }

        private void EnsureCanViewUserMemberByUserId(int targetUserId)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var isOwnProfile = currentUserId == targetUserId;

            EnsureCanViewUserMemberCore(isOwnProfile);
        }

        private void EnsureCanViewUserMemberCore(bool isOwnProfile)
        {
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (isOwnProfile && currentUserRole == "GUEST") return;

            if (currentUserRole == "ADMIN" || currentUserRole == "RECEPTIONIST") return;

            throw new EntityForbiddenException("User",
                "You do not have permission to view this member.");
        }
    }
}
