using OrganizerApi.models;
using OrganizerApi.models.DTOs;
using OrganizerApi.Calendar.Services;
using OrganizerApi.Repository;
using System.Net;

namespace OrganizerApi.Service
{
    public class UserService : IUserService
    {
        private readonly ICalendarService _calendarService;
        private readonly IUserRepository _userRepository;

        public UserService(ICalendarService calService, IUserRepository userRepo) {
            _calendarService = calService;
            _userRepository = userRepo;
        }

        public async Task<AppUser> GetUser(string id)
        {
            return await _userRepository.GetUser(id);
        }

        public async Task<AppUser> SaveOrUpdateUserData(AppUser user)
        {
            return await _userRepository.UpdateUser(user);
        }

        public async Task<HttpResponseMessage> CreateNewUserAsync(NewUserRequest newUserRequest)
        {
            var demoUser = new AppUser
            {
                Id = Guid.NewGuid(),
                Name = newUserRequest.Name,
                EmailAddress = newUserRequest.EmailAddress,
                Password = newUserRequest.Password,
                Calendar = _calendarService.CreateCalendar()
            };

            await _userRepository.SaveNewUser(demoUser);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
