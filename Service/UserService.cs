using OrganizerApi.models;
using OrganizerApi.models.DTOs;

namespace OrganizerApi.Service
{
    public class UserService
    {

        public UserService() { }

        public User CreateNewUser(NewUserRequest newUserRequest)
        {
            var demoUser = new User
            {
                Id = 1,
                Name = newUserRequest.Name,
                EmailAddress = newUserRequest.EmailAddress,
                Password = newUserRequest.Password
            };

            return demoUser;
        }
    }
}
