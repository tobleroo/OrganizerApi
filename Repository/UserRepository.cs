using Microsoft.Azure.Cosmos;
using OrganizerApi.models;

namespace OrganizerApi.Repository
{
    public class UserRepository : IUserRepository
    {

        private readonly Container container;

        public UserRepository()
        {
            var conn = new DbConnection("https://localhost:8081", "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
                "Organizer", "Users");
            container = conn.GetContainer();
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

        public async Task<AppUser> SaveNewUser(AppUser user)
        {
            ItemResponse<AppUser> response = await container.CreateItemAsync<AppUser>(user, new PartitionKey(user.Id.ToString()));
            return response.Resource;
        }

        public async Task<AppUser> UpdateUser(AppUser user)
        {
            ItemResponse<AppUser> response = await container.UpsertItemAsync<AppUser>(user, new PartitionKey(user.Id.ToString()));
            return response.Resource;
        }


    }
}
