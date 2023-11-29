using System.Net;
using OrganizerApi.Auth.models.DTOs;
using OrganizerApi.Auth.models;
using OrganizerApi.Auth.Repository;
using System.Security.Claims;
using Microsoft.Azure.Cosmos;

namespace OrganizerApi.Auth.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepo)
        {
            _userRepository = userRepo;
        }

        public async Task<AppUser> GetUser(string id)
        {
            return await _userRepository.GetUser(id);
        }

        public async Task<AppUser> GetUserByUsername(string username)
        {
            return await _userRepository.GetUserByUsername(username);
        }

        public async Task<AppUser> SaveOrUpdateUserData(AppUser user)
        {
            return await _userRepository.UpdateUser(user);
        }
    }
}
