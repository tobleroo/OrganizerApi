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

        private readonly ICalendarService _calendarService;
        private readonly IUserRepository _userRepository;
        public AuthService(ICalendarService calendarService, IUserRepository userRepository)
        {
            _calendarService = calendarService;
            _userRepository = userRepository;
        }

        public string CreateToken(AppUser user)
        {
            // Create token claims (e.g., user ID and username)
            var claims = new List<Claim>
            {
            new Claim(ClaimTypes.Name, user.Name)
            // Add additional claims as needed
            };

            // Create token credentials and signing key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create token descriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7), // Set token expiration
                SigningCredentials = credentials
            };

            // Create token handler and generate token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Return the generated token as a string
            return tokenHandler.WriteToken(token);
        }

        public void Login(LoginRequest loginReq)
        {
            throw new NotImplementedException();
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
                Name = loginReq.username,
                Password = hashedPassword,
                EmailAddress = "not implemented",
                Calendar = _calendarService.CreateCalendar()
            };


            _userRepository.SaveNewUser(user);
        }
    }
}
