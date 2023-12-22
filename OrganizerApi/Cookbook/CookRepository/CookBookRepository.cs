using Microsoft.Azure.Cosmos;
using OrganizerApi.Auth.Repository;
using OrganizerApi.Cookbook.Config;
using OrganizerApi.Cookbook.CookModels;
using OrganizerApi.Cookbook.CookModels.CookbookDTOs;
using OrganizerApi.Cookbook.CookModels.CookbookDTOs.shoppinglist;
using System.Net;

namespace OrganizerApi.Cookbook.CookRepository
{
    public class CookBookRepository : ICookBookRepository
    {

        private Container container;

        public CookBookRepository(CosmosClient cosmosClient, string databaseId, string containerId, int? throughput = null)
        {
            var database = cosmosClient.GetDatabase(databaseId);
            CreateContainerIfNotExistsAsync(database, containerId, throughput).GetAwaiter().GetResult();
            container = database.GetContainer(containerId);
        }

        private async Task CreateContainerIfNotExistsAsync(Database database, string containerId, int? throughput = null)
        {
            try
            {
                await database.CreateContainerIfNotExistsAsync(containerId, "/id", throughput);
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                // Handle exception if the database does not exist
            }
        }


        public async Task<UserCookBook>? GetCookBook(string username)
        {

            try
            {
                var sqlQueryText = $"SELECT * FROM c WHERE c.OwnerUsername = '{username}'";
                QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
                FeedIterator<UserCookBook> queryResultSetIterator = container.GetItemQueryIterator<UserCookBook>(queryDefinition);

                while (queryResultSetIterator.HasMoreResults)
                {
                    FeedResponse<UserCookBook> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                    foreach (UserCookBook userCookBook in currentResultSet)
                    {
                        return userCookBook;
                    }
                }

                //if there is no user cookbook, create new and return
                return new UserCookBook() { OwnerUsername = username
                };
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error in GetCookBook repository method", ex);
            }

        }

        //create or update a cookbook
        public async Task<bool> UpdateCookBook(UserCookBook cookbook)
        {
            try
            {
                ItemResponse<UserCookBook> response = await container.UpsertItemAsync(cookbook, new PartitionKey(cookbook.id.ToString()));

                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new ApplicationException("Error in UpdateCookBook repository method", ex);
            }
        }

        public async Task<SingleShopList?> GetShoppingList(string username, string cookbookId)
        {
           
            string sqlQueryText = @"
                SELECT VALUE shopList
                FROM c
                JOIN shopList IN c.ShoppingLists
                WHERE c.OwnerUsername = @ownerUsername AND shopList.ListName = 'Shopping list'";

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText)
                .WithParameter("@ownerUsername", username);

            FeedIterator<SingleShopList> queryResultSetIterator = container.GetItemQueryIterator<SingleShopList>(
                queryDefinition,
                requestOptions: new QueryRequestOptions { PartitionKey = new PartitionKey(cookbookId) }
            );

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<SingleShopList> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                if (currentResultSet.Count > 0)
                {
                    return currentResultSet.FirstOrDefault();
                }
            }

            return null;
        }
        public async Task<string> FetchUserCookbookId(string username)
        {
            //create sql to get coobook id from username
            var sqlQueryText = $"SELECT VALUE c.id FROM c WHERE c.OwnerUsername = @ownerUsername";
            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText).WithParameter("@ownerUsername", username);
            using FeedIterator<string> resultSetIterator = container.GetItemQueryIterator<string>(queryDefinition);
            if (resultSetIterator.HasMoreResults)
            {
                FeedResponse<string> response = await resultSetIterator.ReadNextAsync();
                foreach (var id in response)
                {
                    return id;
                }
            }

            return null; // Return null if no document was found
        }

        public async Task<List<RecipeOverviewData>> FetchRecipiesOverview(string username)
        {
            var sqlQueryText = $"SELECT r.Guid AS Id, r.RecipeName AS Name, r.CookTime AS TimeToCook, r.RecipeType, r.Difficulty FROM c JOIN r IN c.Recipes WHERE c.OwnerUsername = @username";
            var queryDefinition = new QueryDefinition(sqlQueryText).WithParameter("@username", username);
            var queryResultSetIterator = container.GetItemQueryIterator<RecipeOverviewData>(queryDefinition);
            var recipes = new List<RecipeOverviewData>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<RecipeOverviewData> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (RecipeOverviewData recipe in currentResultSet)
                {
                    recipes.Add(recipe);
                }
            }

            return recipes;
        }

        public async Task<Recipe> FetchOneRecipe(string username, string recipeId)
        {
            try
            {
                var sqlQueryText = @"
                SELECT VALUE r
                FROM c
                JOIN r IN c.Recipes
                WHERE c.OwnerUsername = @username AND r.Guid = @recipeId";

                var queryDefinition = new QueryDefinition(sqlQueryText)
                    .WithParameter("@username", username)
                    .WithParameter("@recipeId", recipeId);

                var queryResultSetIterator = container.GetItemQueryIterator<Recipe>(queryDefinition);

                if (queryResultSetIterator.HasMoreResults)
                {
                    FeedResponse<Recipe> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                    foreach (Recipe recipe in currentResultSet)
                    {
                        return recipe;
                    }
                }

                return null; // If no recipe is found.
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new ApplicationException("Error in FetchOneRecipe method", ex);
            }
        }
    }
}
