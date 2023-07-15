using Microsoft.Azure.Cosmos;
using Microsoft.IdentityModel.Tokens;
using OrganizerApi.Auth.models;
using OrganizerApi.Auth.models.DTOs;
using OrganizerApi.Auth.Repository;
using OrganizerApi.Calendar.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OrganizerApi.Auth.AuthService
{
    public class AuthService : IAuthService
    {

        private readonly IConfiguration _configuration;
        private readonly ICalendarService _calendarService;
        private readonly IUserRepository _userRepository;
        private readonly string _secretKey = "2e7b57ad2498e34f0b6405bb29c9959fcf63f2f89dc13e27a4dd524d9c16d9e2";
        public AuthService(ICalendarService calendarService, IUserRepository userRepository, IConfiguration config)
        {
            _calendarService = calendarService;
            _userRepository = userRepository;
            _configuration = config;
        }

        public string CreateJwtToken(AppUser user)
        {
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Role, "User"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                "2e7b57ad2498e34f0b6405bb29c9959fcf63f2f89dc13e27a4dd524d9c16d9e2"));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public async Task<AppUser> Login(LoginRequest loginReq)
        {
            // find user in db by username
            var user = await _userRepository.GetUserByUsername(loginReq.username);
            
            if (user == null)
            {
                throw new Exception("User not found");
            }
            else
            {
                // verify password
                if (!BCrypt.Net.BCrypt.Verify(loginReq.password, user.Password))
                {
                    throw new Exception("Invalid password");
                }
                else
                {
                    return user;
                }
            }

        }

        public void Register(NewUserRequest newUser)
        {
            //check that email or username doesnt already exists
            var checkUsername = _userRepository.GetUserByUsername(newUser.Name);
            var checkEmail = _userRepository.GetUserByEmail(newUser.EmailAddress);
            if (checkUsername != null && checkEmail != null)
            {
                throw new Exception("The username and email address you provided already exist.");
            }

            if (newUser.registrationCode != "jeffkaff2000")
            {
                throw new Exception("not the right code!")
            }


            // Hash the password using Bcrypt
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newUser.password);
            

            // Example code:
            var user = new AppUser
            {
                Id = Guid.NewGuid(),
                Name = loginReq.username,
                Password = hashedPassword,
                EmailAddress = newUser.EmailAddress,
                Calendar = _calendarService.CreateCalendar()
            };


            _userRepository.SaveNewUser(user);
        }
    }
}
