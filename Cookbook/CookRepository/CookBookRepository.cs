using Microsoft.Azure.Cosmos;
using OrganizerApi.Auth.Repository;
using OrganizerApi.Cookbook.CookModels;
using System.Net;

namespace OrganizerApi.Cookbook.CookRepository
{
    public class CookBookRepository : ICookBookRepository
    {

        private Container container;

        public CookBookRepository() => InitializeContainerAsync().Wait();

        private async Task InitializeContainerAsync()
        {
            var conn = new DbConnection("https://tobleroo.documents.azure.com:443/", "8xGGAueMy7XRGYeCA8nj2mwZ6w0gDhNffoIeaWDIHdC7dMfjTght7zdQAv9zs7gZvAWM2nUMhernACDbozvSMg==",
                "Organizer", "cookbook");
            container = await conn.GetContainer(); // Await the Task<Container> object to get the Container
        }

        public async Task<UserCookBook>? GetCookBook(string username)
        {
            //retrieve the cookbook from the database using the username
            var sqlQueryText = $"SELECT * FROM c WHERE c.OwnerUsername = '{username}'";
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

        //create method to createor update a cookbook
        public async Task<bool> UpdateCookBook(UserCookBook cookbook)
        {
            try
            {
                ItemResponse<UserCookBook> response = await container.UpsertItemAsync(cookbook, new PartitionKey(cookbook.id.ToString()));

                // Check the status code to determine success or failure.
                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
                {
                    return true; // Operation succeeded
                }
                else
                {
                    return false; // Operation failed
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false; // Operation failed due to an exception
            }
        }
    }
}
