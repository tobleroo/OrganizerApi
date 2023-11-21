using OrganizerApi.Calendar.Services;
using OrganizerApi.Auth.models.DTOs;
using OrganizerApi.Auth.models;
using System.Security.Claims;

namespace OrganizerApi.Auth.UserService
{
    public interface IUserService
    {

        Task<AppUser> GetUser(string id);
        Task<AppUser> GetUserByUsername(string username);
        Task<AppUser> SaveOrUpdateUserData(AppUser user);

    }
}
