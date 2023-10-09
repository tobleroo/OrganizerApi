using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrganizerApi.Auth.AuthService;
using OrganizerApi.Auth.DTOs.models;
using OrganizerApi.Auth.models.DTOs;

namespace OrganizerApi.Auth.AuthControllers
{
    [ApiController]
    [Route("/auth")]
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
        public async Task<IActionResult> Register(NewUserRequest loginReq)
        {
            try
            {
                var resultString = await _authService.Register(loginReq);
                if (resultString != "user saved!") 
                { 
                    return BadRequest(resultString);
                }

                return Ok(resultString);
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
            if (userExists.Result == null)
            {
                return Unauthorized("not the right username or password!");
            }
            else
            {
                var token = _authService.CreateJwtToken(userExists.Result);
                
                return Ok(token);
            }
        }
    }
}
