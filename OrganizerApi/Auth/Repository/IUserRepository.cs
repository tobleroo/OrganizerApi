using OrganizerApi.Auth.models;

namespace OrganizerApi.Auth.Repository
{
    public interface IUserRepository
    {

        Task<AppUser> GetUser(string id);
        Task<bool> SaveNewUser(AppUser user);

        Task<AppUser> GetUserByUsername(string username);

        Task<AppUser> UpdateUser(AppUser user);

        Task<AppUser> GetUserByEmail(string email);

        Task<bool> CheckIfUsernameExists(string username);

        Task<bool> CheckIfEmailExists(string email);
        
        Task<bool> CheckEmailAndUsernameExists(string email, string username);

    }
}
