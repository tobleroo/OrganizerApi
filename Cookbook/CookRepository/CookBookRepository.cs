using Microsoft.Azure.Cosmos;
using OrganizerApi.Auth.Repository;
using OrganizerApi.Cookbook.CookModels;

namespace OrganizerApi.Cookbook.CookRepository
{
    public class CookBookRepository : ICookBookRepository
    {

        private Container container;

        public CookBookRepository() => InitializeContainerAsync().Wait();

        private async Task InitializeContainerAsync()
        {
            var conn = new DbConnection("https://localhost:8081", "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
                "Organizer", "cookbook");
            container = await conn.GetContainer(); // Await the Task<Container> object to get the Container
        }

        public async Task<UserCookBook>? GetCookBook(string username)
        {
            //retrieve the cookbook from the database using the username
            var sqlQueryText = $"SELECT * FROM c WHERE c.Owner = '{username}'";
            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<UserCookBook> queryResultSetIterator = container.GetItemQueryIterator<UserCookBook>(queryDefinition);

            //iterate over the results and return the first one
            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<UserCookBook> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (UserCookBook userCookBook in currentResultSet)
                {
                    return userCookBook;
                }
            }
            return null;
           
        }

        public async Task<UserCookBook> SaveNewCookBook(UserCookBook cookbook)
        {
            ItemResponse<UserCookBook> response = await container.CreateItemAsync(cookbook, new PartitionKey(cookbook.id.ToString()));
            return response.Resource;
        }
    }
}
