using Microsoft.Azure.Cosmos;
using OrganizerApi.Auth.Config;
using OrganizerApi.Auth.models;
using OrganizerApi.Cookbook.Config;
using System.Net;

namespace OrganizerApi.Auth.Repository
{
    public class UserRepository : IUserRepository
    {

        private Container container;

        public UserRepository(Container container)
        {
            this.container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public async Task<AppUser> GetUser(string id)
        {
            try
            {
                ItemResponse<AppUser> response = await container.ReadItemAsync<AppUser>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<AppUser> GetUserByUsername(string username)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username))
                {
                    // Handle or log invalid input as needed
                    return null;
                }

                var query = new QueryDefinition("SELECT * FROM c WHERE c.Name = @username")
                    .WithParameter("@username", username);

                var iterator = container.GetItemQueryIterator<AppUser>(query);
                var response = await iterator.ReadNextAsync();

                return response.Count > 0 ? response.First() : null;
            }
            catch (CosmosException ex)
            {
                throw; 
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> SaveNewUser(AppUser user)
        {
            try
            {
                await container.CreateItemAsync(user, new PartitionKey(user.Id.ToString()));
                return true; // Item created successfully
            }
            catch (CosmosException e)
            {
                if (e.StatusCode == HttpStatusCode.Conflict)
                {
                    // Item with the same ID already exists
                    return false;
                }
                else
                {
                    // Other error occurred during item creation
                    throw;
                }
            }
        }

        public async Task<AppUser> UpdateUser(AppUser user)
        {
            ItemResponse<AppUser> response = await container.UpsertItemAsync(user, new PartitionKey(user.Id.ToString()));
            return response.Resource;
        }

        // get user by email
        public async Task<AppUser> GetUserByEmail(string email)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.EmailAddress = @email")
                .WithParameter("@email", email);

            var iterator = container.GetItemQueryIterator<AppUser>(query);
            var response = await iterator.ReadNextAsync();

            return response.Resource.FirstOrDefault();
        }

        public async Task<bool> CheckIfUsernameExists(string username)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.Name = @name")
                .WithParameter("@name", username);

            var iterator = container.GetItemQueryIterator<AppUser>(query);
            var response = await iterator.ReadNextAsync();

            //returns true if user exists
            return response.Resource != null;
        }

        public async Task<bool> CheckIfEmailExists(string email)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.EmailAddress = @email")
                .WithParameter("@email", email);

            var iterator = container.GetItemQueryIterator<AppUser>(query);
            var response = await iterator.ReadNextAsync();

            //returns true if user exists
            return response.Resource != null;
        }

        public async Task<bool> CheckEmailAndUsernameExists(string email, string username)
        {

            var query = new QueryDefinition("SELECT VALUE COUNT(1) FROM c WHERE c.Name = @username OR c.EmailAddress = @email")
                .WithParameter("@username", username)
                .WithParameter("@email", email);

            var iterator = container.GetItemQueryIterator<int>(query);
            var response = await iterator.ReadNextAsync();

            int count = response.FirstOrDefault();
            return count > 0;
        }
    }
}
