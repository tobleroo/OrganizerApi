using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.IdentityModel.Tokens;
using OrganizerApi.Auth.DTOs.models;
using OrganizerApi.Auth.models;
using OrganizerApi.Auth.models.DTOs;
using OrganizerApi.Auth.Repository;
using OrganizerApi.Auth.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OrganizerApi.Auth.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public string CreateJwtToken(AppUser user)
        {
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, "User"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                "2e7b57ad2498e34f0b6405bb29c9959fcf63f2f89dc13e27a4dd524d9c16d9e2"));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(7),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public async Task<AppUser?> Login(LoginRequest loginReq)
        {
            // find user in db by username
            var user = await _userRepository.GetUserByUsername(loginReq.Username);

            if (user == null)
            {
                return null;
            }
            else
            {
                // verify password
                if (!BCrypt.Net.BCrypt.Verify(loginReq.Password, user.Password))
                {
                    return null;
                }
                else
                {
                    return user;
                }
            }

        }

        public async Task<string> Register(NewUserRequest newUser)
        {
            //check that email or username doesnt already exists
            if(!AuthChecks.CheckValidEmail(newUser.EmailAddress))
            {
                return "email not valid!";
            }
            var checkUsernameAndEmail = await _userRepository.CheckEmailAndUsernameExists(newUser.EmailAddress.ToLower(), newUser.Name);

            if (checkUsernameAndEmail)
            {
                return "User already exists";
            }

            if (newUser.RegistrationCode != "jeffkaff2000")
            {
                return "not right code!";
            }


            // Hash the password using Bcrypt
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
            

            // Example code:
            var user = new AppUser
            {
                Id = Guid.NewGuid(),
                Name = newUser.Name,
                Password = hashedPassword,
                EmailAddress = newUser.EmailAddress,
            };

            var success  = await _userRepository.SaveNewUser(user);

            if (success) return "user saved!";
                else return "";
        }

        
    }
}
