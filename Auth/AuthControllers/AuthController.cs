using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrganizerApi.Auth.AuthService;
using OrganizerApi.Auth.models;

namespace OrganizerApi.Auth.AuthControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }
        [HttpPost("register")]
        public IActionResult Register(LoginRequest loginReq)
        {
            try
            {
                _authService.Register(loginReq);
                return Ok("successfully registered");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest loginReq)
        {
            var userExists = _authService.Login(loginReq);
            if (userExists == null)
            {
                return Unauthorized();
            }
            else
            {
                var token = _authService.CreateJwtToken(userExists.Result);
                return Ok(token);
            }
        }
    }
}
