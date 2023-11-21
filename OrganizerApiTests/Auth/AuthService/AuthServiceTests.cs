using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OrganizerApi.Auth.AuthService;
using OrganizerApi.Auth.DTOs.models;
using OrganizerApi.Auth.models;
using OrganizerApi.Auth.models.DTOs;
using OrganizerApi.Auth.Repository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OrganizerApi.Auth.AuthService.Tests
{
    [TestClass()]
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            // Mock other dependencies as needed
            _authService = new AuthService(_mockUserRepository.Object);
        }

        [TestMethod()]
        public async Task Login_WithCorrectPassword()
        {
            // Arrange
            var loginReq = new LoginRequest { Username = "testuser", Password = "correctpassword" };
            var user = new AppUser { Name = "testuser", Password = BCrypt.Net.BCrypt.HashPassword("correctpassword") };

            _mockUserRepository.Setup(repo => repo.GetUserByUsername("testuser"))
                               .ReturnsAsync(user);

            // Act
            var result = await _authService.Login(loginReq);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public async Task Register_WithValidData_ShouldReturnUserSaved()
        {
            // Arrange
            var newUserRequest = new NewUserRequest
            (
                "NewUser",
                "newuser@example.com",
                "password123",
                "jeffkaff2000"
            );

            _mockUserRepository.Setup(repo => repo.CheckEmailAndUsernameExists(It.IsAny<string>(), It.IsAny<string>()))
                               .ReturnsAsync(false); // Simulate that user does not already exist

            _mockUserRepository.Setup(repo => repo.SaveNewUser(It.IsAny<AppUser>()))
                               .ReturnsAsync(true); // Simulate successful user save

            // Act
            var result = await _authService.Register(newUserRequest);

            // Assert
            Assert.AreEqual("user saved!", result);
        }

        [TestMethod()]
        public void CreateJwtToken_ShouldReturnValidToken()
        {
            // Arrange
            var user = new AppUser
            {
                Name = "TestUser"
            };

            // Act
            var token = _authService.CreateJwtToken(user);

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(token), "Token should not be null or empty");

            // Optional: Validate the token structure and claims
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            Assert.IsTrue(jwtToken.Claims.Any(c => c.Type == ClaimTypes.Name && c.Value == user.Name), "Token should contain the correct name claim");
            Assert.IsTrue(jwtToken.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "User"), "Token should contain the role claim");
            Assert.IsTrue(jwtToken.ValidTo > DateTime.UtcNow, "Token expiry should be in the future");
        }


    }
}