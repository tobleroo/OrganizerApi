using OrganizerApi.Auth.models;
using OrganizerApi.models;

namespace OrganizerApi.Auth.AuthService
{
    public interface IAuthService
    {

        void Register(LoginRequest loginReq);
        string CreateJwtToken(AppUser user);
        Task<AppUser> Login(LoginRequest loginReq);
    }
}
