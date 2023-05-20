using OrganizerApi.models;
using OrganizerApi.models.DTOs;
using OrganizerApi.Calendar.Services;

namespace OrganizerApi.Service
{
    public interface IUserService
    {

        Task<AppUser> GetUser(string id);
        Task<HttpResponseMessage> CreateNewUserAsync(NewUserRequest newUserRequest);
        Task<AppUser> SaveOrUpdateUserData(AppUser user);
    }
}
