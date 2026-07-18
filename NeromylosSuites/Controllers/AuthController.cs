using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeromylosSuites.DTO;
using NeromylosSuites.Services;

namespace NeromylosSuites.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IApplicationService _applicationService;
        private readonly IConfiguration _configuration;

        public AuthController(IApplicationService applicationService, IConfiguration configuration)
        {
            _applicationService = applicationService;
            _configuration = configuration;
        }

        /// <summary>
        /// Sign up a new member.
        /// </summary>
        /// <param name="memberSignupDTO">The member details</param>
        /// <returns>The created member-user</returns>
        /// <response code="201">Returns the created member-user.</response>
        /// <response code="400">If the request body is invalid.</response>
        /// <response code="409">If a member-user with the same username or email already exists.</response>
        [HttpPost("register/member")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UserReadOnlyDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<UserReadOnlyDTO>> RegisterMember(
            [FromBody] MemberSignupDTO memberSignupDTO)
        {
            var createMember = await _applicationService.MemberService
                .SignUpMemberAsync(memberSignupDTO);

            return CreatedAtAction(
                actionName: nameof(UsersController.GetUserById),
                controllerName: "Users",
                routeValues: new { id = createMember.Id },
                value: createMember);
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token.
        /// </summary>
        /// <param name="credentials">The user credentials</param>
        /// <returns>The Jwt token.</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(JwtTokenDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<JwtTokenDTO>> Login([FromBody] UserLoginDTO credentials)
        {
            var result = await _applicationService.UserService.LoginAsync(credentials);
            return Ok(new JwtTokenDTO(result.Token));
        }
    }
}
