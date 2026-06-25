using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeromylosSuites.DTO;
using NeromylosSuites.Exceptions;
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
        /// Registers a new member account.
        /// </summary>
        [HttpPost("register/member")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UserReadOnlyDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<UserReadOnlyDTO>> RegisterMember(
            [FromBody] MemberSignupDTO memberSignupDTO)
        {
            var createUser = await _applicationService.MemberService
                .SignUpUserAsync(memberSignupDTO);

            return CreatedAtAction(
                actionName: nameof(UsersController.GetUserById),
                controllerName: "Users",
                routeValues: new { id = createUser.Id },
                value: createUser);
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token.
        /// </summary>
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
