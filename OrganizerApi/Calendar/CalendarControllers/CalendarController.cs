using Microsoft.AspNetCore.Mvc;
using OrganizerApi.Calendar.Models.CalendarDTOs;
using OrganizerApi.Calendar.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using OrganizerApi.Calendar.Models;
using OrganizerApi.Auth.UserService;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;

namespace OrganizerApi.Calendar.CalendarControllers
{

    [ApiController]
    [Route("/calendar")]
    [Authorize]
    public class CalendarController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly ICalendarService _calendarService;

        public CalendarController(IUserService userService, ICalendarService calendarService)
        {
            _userService = userService;
            _calendarService = calendarService;

        }

        [HttpGet]
        public async Task<IActionResult> getCalendar()
        {
            var name = User.FindFirstValue(ClaimTypes.Name);
            var user = _userService.GetUserByUsername(name);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user.Result.Calendar);
        }


        [HttpPost("task/delete")]
        public async Task<IActionResult?> deleteTaskFromCalendar([FromBody] NewTaskDTO deleteTaskDTO)
        {
            // Find the user by ID (assuming you have a UserService or UserRepository to retrieve the user)
            var name = User.FindFirstValue(ClaimTypes.Name);
            var user = await _userService.GetUserByUsername(name);
            if (user == null)
            {
                return NotFound("user not found!"); // return 404 if user not found
            }
            _calendarService.RemoveTaskFromCalendar(user.Calendar, deleteTaskDTO);
            await _userService.SaveOrUpdateUserData(user);
            return Ok("task deleted from calendar!");
        }

        [HttpPost("task/update")]
        public async Task<IActionResult?> updateTaskInCalendar([FromBody] UpdateCalendarTaskDTO updateCalendarTask)
        {
            // Find the user by ID (assuming you have a UserService or UserRepository to retrieve the user)
            var name = User.FindFirstValue(ClaimTypes.Name);
            var user = await _userService.GetUserByUsername(name);
            if (user == null)
            {
                return NotFound("user not found!"); // return 404 if user not found
            }
            _calendarService.UpdateTaskInCalendar(user.Calendar, updateCalendarTask);
            await _userService.SaveOrUpdateUserData(user);
            return Ok("task updated in calendar!");
        }

        [HttpPost("task/new")]
        public async Task<IActionResult?> addNewTaskToCalendar([FromBody] NewTaskDTO newTaskDTO) 
        {
            var name = User.FindFirstValue(ClaimTypes.Name);
            var user = await _userService.GetUserByUsername(name);

            if (user == null)
            {
                return NotFound("user not found!"); // return 404 if user not found
            }

            _calendarService.AddTaskToCalendar(user.Calendar, newTaskDTO);

            await _userService.SaveOrUpdateUserData(user);

            return Ok("task added to calendar! ");
        }

        [HttpPost("event/add")]
        public async Task<IActionResult?> addNewEventToCalendar([FromBody] NewEventDTO newEventDTO)
        {
            // Find the user by ID (assuming you have a UserService or UserRepository to retrieve the user)
            var name = User.FindFirstValue(ClaimTypes.Name);
            var user = await _userService.GetUserByUsername(name);
            if (user == null)
            {
                return NotFound("user not found!"); // return 404 if user not found
            }
            _calendarService.AddNewEventToCalendar(user.Calendar, newEventDTO);
            await _userService.SaveOrUpdateUserData(user);
            return Ok("event added to calendar!");
        }

        [HttpDelete("event/delete")]
        public async Task<IActionResult?> deleteEventFromCalendar([FromBody] NewEventDTO deleteEventDTO)
        {
            var name = User.FindFirstValue(ClaimTypes.Name);
            var user = await _userService.GetUserByUsername(name);
            if (user == null)
            {
                return NotFound("user not found!"); // return 404 if user not found
            }
            _calendarService.RemoveEventFromCalendar(user.Calendar, deleteEventDTO);
            await _userService.SaveOrUpdateUserData(user);
            return Ok("event deleted from calendar!");
        }

        [HttpPost("update")]
        public async Task<IActionResult?> updateCalendar([FromBody] List<CalendarDay> calendar)
        {
            var name = User.FindFirstValue(ClaimTypes.Name);
            var user = await _userService.GetUserByUsername(name);
            if (user == null)
            {
                return NotFound("user not found!"); // return 404 if user not found
            }
            _calendarService.UpdateCalendar(user, calendar);
            await _userService.SaveOrUpdateUserData(user);
            return Ok("calendar updated!");
        }

    }
}
