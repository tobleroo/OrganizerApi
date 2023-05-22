using OrganizerApi.Auth.models;
using OrganizerApi.models;

namespace OrganizerApi.Auth.AuthService
{
    public interface IAuthService
    {

        void Register(LoginRequest loginReq);
        string CreateToken(AppUser user);
        void Login(LoginRequest loginReq);
    }
}
