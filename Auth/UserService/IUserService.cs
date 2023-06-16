using OrganizerApi.Calendar.Services;
using OrganizerApi.Auth.models.DTOs;
using OrganizerApi.Auth.models;

namespace OrganizerApi.Auth.UserService
{
    public interface IUserService
    {

        Task<AppUser> GetUser(string id);
        Task<HttpResponseMessage> CreateNewUserAsync(NewUserRequest newUserRequest);
        Task<AppUser> SaveOrUpdateUserData(AppUser user);
    }
}
