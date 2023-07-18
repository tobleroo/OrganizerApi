using Microsoft.AspNetCore.Mvc;
using OrganizerApi.Auth.DTOs.models;
using OrganizerApi.Auth.models;
using OrganizerApi.Auth.models.DTOs;

namespace OrganizerApi.Auth.AuthService
{
    public interface IAuthService
    {

        Task<string> Register(NewUserRequest newUser);
        string CreateJwtToken(AppUser user);
        Task<AppUser> Login(LoginRequest loginReq);
        
    }
}
