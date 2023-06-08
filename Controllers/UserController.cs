//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using OrganizerApi.models.DTOs;
//using OrganizerApi.Service;

//// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace OrganizerApi.Controllers
//{
//    [Route("/user")]
//    [ApiController]
//    public class UserController : ControllerBase
//    {

//        private readonly IUserService _userService;

//        public UserController(IUserService userService)
//        {
//            _userService = userService;
//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetSpecificUser(string id)
//        {
//            var user = await _userService.GetUser(id);

//            if (user == null)
//            {
//                return NotFound(); // return 404 if user not found
//            }

//            return Ok(user);
//        }

//        [HttpPost("new")]
//        public async Task<IActionResult?> CreateNewUserAsync([FromBody] NewUserRequest newUserRequest)
//        {
//            var newUser = await _userService.CreateNewUserAsync(newUserRequest);

//            return Ok(newUser);
//        }

//    }
//}
