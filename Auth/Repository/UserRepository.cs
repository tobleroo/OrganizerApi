using Microsoft.Azure.Cosmos;
using OrganizerApi.Auth.models;

namespace OrganizerApi.Auth.Repository
{
    public class UserRepository : IUserRepository
    {

        private Container container;

        public UserRepository()
        {
            InitializeContainerAsync().Wait();
        }

        private async Task InitializeContainerAsync()
        {
            var conn = new DbConnection("https://localhost:8081", "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
                "Organizer", "Users");
            container = await conn.GetContainer(); // Await the Task<Container> object to get the Container
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
            var query = new QueryDefinition("SELECT * FROM c WHERE c.Name = @username")
                .WithParameter("@username", username);

            var iterator = container.GetItemQueryIterator<AppUser>(query);
            var response = await iterator.ReadNextAsync();

            return response.Resource.First();
        }

        public async Task<AppUser> SaveNewUser(AppUser user)
        {
            ItemResponse<AppUser> response = await container.CreateItemAsync(user, new PartitionKey(user.Id.ToString()));
            return response.Resource;
        }

        public async Task<AppUser> UpdateUser(AppUser user)
        {
            ItemResponse<AppUser> response = await container.UpsertItemAsync(user, new PartitionKey(user.Id.ToString()));
            return response.Resource;
        }


    }
}
