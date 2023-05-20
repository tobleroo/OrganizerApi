using Microsoft.AspNetCore.Mvc;
using OrganizerApi.Calendar.Models.CalendarDTOs;
using OrganizerApi.Calendar.Services;
using OrganizerApi.models.DTOs;
using OrganizerApi.Service;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using OrganizerApi.Calendar.Models;

namespace OrganizerApi.Calendar.CalendarControllers
{

    [Route("/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly ICalendarService _calendarService;

        public CalendarController(IUserService userService, ICalendarService calendarService)
        {
            _userService = userService;
            _calendarService = calendarService;

        }

        [HttpPost("task/delete")]
        public async Task<IActionResult?> deleteTaskFromCalendar(string userId, [FromBody] NewTaskDTO deleteTaskDTO)
        {
            // Find the user by ID (assuming you have a UserService or UserRepository to retrieve the user)
            var user = await _userService.GetUser(userId);
            if (user == null)
            {
                return NotFound("user not found!"); // return 404 if user not found
            }
            _calendarService.RemoveTaskFromCalendar(user.Calendar, deleteTaskDTO);
            await _userService.SaveOrUpdateUserData(user);
            return Ok("task deleted from calendar!");
        }

        [HttpPost("task/update")]
        public async Task<IActionResult?> updateTaskInCalendar(string userId, [FromBody] UpdateCalendarTaskDTO updateCalendarTask)
        {
            // Find the user by ID (assuming you have a UserService or UserRepository to retrieve the user)
            var user = await _userService.GetUser(userId);
            if (user == null)
            {
                return NotFound("user not found!"); // return 404 if user not found
            }
            _calendarService.UpdateTaskInCalendar(user.Calendar, updateCalendarTask);
            await _userService.SaveOrUpdateUserData(user);
            return Ok("task updated in calendar!");
        }

        [HttpPost("task/new")]
        public async Task<IActionResult?> addNewTaskToCalendar(string userId,[FromBody] NewTaskDTO newTaskDTO) 
        {
            // Find the user by ID (assuming you have a UserService or UserRepository to retrieve the user)
            var user = await _userService.GetUser(userId);

            if (user == null)
            {
                return NotFound("user not found!"); // return 404 if user not found
            }

            _calendarService.AddTaskToCalendar(user.Calendar, newTaskDTO);

            await _userService.SaveOrUpdateUserData(user);
            return Ok("task added to calendar!");
        }

        [HttpPost("event/add")]
        public async Task<IActionResult?> addNewEventToCalendar(string userId, [FromBody] NewEventDTO newEventDTO)
        {
            // Find the user by ID (assuming you have a UserService or UserRepository to retrieve the user)
            var user = await _userService.GetUser(userId);
            if (user == null)
            {
                return NotFound("user not found!"); // return 404 if user not found
            }
            _calendarService.AddNewEventToCalendar(user.Calendar, newEventDTO);
            await _userService.SaveOrUpdateUserData(user);
            return Ok("event added to calendar!");
        }

        [HttpDelete("event/delete")]
        public async Task<IActionResult?> deleteEventFromCalendar(string userId, [FromBody] NewEventDTO deleteEventDTO)
        {
            // Find the user by ID (assuming you have a UserService or UserRepository to retrieve the user)
            var user = await _userService.GetUser(userId);
            if (user == null)
            {
                return NotFound("user not found!"); // return 404 if user not found
            }
            _calendarService.RemoveEventFromCalendar(user.Calendar, deleteEventDTO);
            await _userService.SaveOrUpdateUserData(user);
            return Ok("event deleted from calendar!");
        }

        [HttpPost("update")]
        public async Task<IActionResult?> updateCalendar(string userId, [FromBody] List<CalendarDay> calendar)
        {
            // Find the user by ID (assuming you have a UserService or UserRepository to retrieve the user)
            var user = await _userService.GetUser(userId);
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
