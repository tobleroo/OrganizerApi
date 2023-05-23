using Microsoft.Azure.Cosmos;
using Microsoft.IdentityModel.Tokens;
using OrganizerApi.Auth.models;
using OrganizerApi.Calendar.Services;
using OrganizerApi.models;
using OrganizerApi.Repository;
using OrganizerApi.Service;
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

        public void Register(LoginRequest loginReq)
        {

            // Hash the password using Bcrypt
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(loginReq.password);

            // Save the username and hashed password to the database or user store
            // Replace this with your actual implementation

            // Example code:
            var user = new AppUser
            {
                Id = Guid.NewGuid(),
                Name = loginReq.username,
                Password = hashedPassword,
                EmailAddress = "not implemented",
                Calendar = _calendarService.CreateCalendar()
            };


            _userRepository.SaveNewUser(user);
        }
    }
}
