using Microsoft.Azure.Cosmos;
using OrganizerApi.Auth.Repository;
using OrganizerApi.Cookbook.CookModels;
using OrganizerApi.Cookbook.CookModels.CookbookDTOs;
using System.Net;

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

        public async Task<ShoppingListALLItems?> GetShoppingList(string username)
        {
            // Define the SQL query to fetch the entire UserCookBook for the given username
            var sqlQueryText = $"SELECT * FROM c WHERE c.OwnerUsername = '{username}'";
            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);

            FeedIterator<UserCookBook> queryResultSetIterator = container.GetItemQueryIterator<UserCookBook>(queryDefinition);

            ShoppingListALLItems? shoppingListALLItems = null;

            // Iterate over the results (assuming a user will have only one UserCookBook)
            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<UserCookBook> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (UserCookBook cookbook in currentResultSet)
                {
                    shoppingListALLItems = new ShoppingListALLItems()
                    {
                        ShoppingList = cookbook.ShoppingList.FirstOrDefault(), // Assuming you want the first shopping list
                        EarlierAddedAdditionalItems = cookbook.PreviouslyAddedAdditonalItems
                    };
                }
            }

            return shoppingListALLItems;
        }

        public async Task<bool> UpsertAdditionalItemsShoppingList(string username, List<string> shoppinglistToUpdate)
        {
            // Step 1: Get the UserCookBook associated with the given username
            UserCookBook? userCookBook = await GetCookBook(username);

            if (userCookBook == null)
            {
                // Handle the case where no UserCookBook exists for the given username
                return false;
            }

            userCookBook.PreviouslyAddedAdditonalItems = shoppinglistToUpdate;
            await UpdateCookBook(userCookBook);
            return true;

            
        }
    }
}
