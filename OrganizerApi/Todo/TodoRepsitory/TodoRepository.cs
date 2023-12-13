using Microsoft.Azure.Cosmos;
using OrganizerApi.Diary.models;
using OrganizerBlazor.Todo.Models;

namespace OrganizerApi.Todo.TodoRepsitory
{
    public class TodoRepository : ITodoRepository
    {

        private Container container;

        public TodoRepository(CosmosClient cosmosClient, string databaseId, string containerId, int? throughput = null)
        {
            var database = cosmosClient.GetDatabase(databaseId);
            CreateContainerIfNotExistsAsync(database, containerId, throughput).GetAwaiter().GetResult();
            container = database.GetContainer(containerId);
        }

        public async Task CreateContainerIfNotExistsAsync(Database database, string containerId, int? throughput = null)
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

        public async Task<TodoDocument> GetTodo(string username)
        {
            try
            {
                var sqlQueryText = $"SELECT * FROM c WHERE c.Owner = '{username}'";
                QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
                FeedIterator<TodoDocument> queryResultSetIterator = container.GetItemQueryIterator<TodoDocument>(queryDefinition);

                while (queryResultSetIterator.HasMoreResults)
                {
                    FeedResponse<TodoDocument> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                    foreach (TodoDocument todo in currentResultSet)
                    {
                        return todo;
                    }
                }

                //if there is no user cookbook, create new and return
                return new TodoDocument()
                {
                    Owner = username
                };
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error in diary repository method", ex);
            }

        }

        public async Task<bool> UpsertTodo(TodoDocument todoDoc)
        {
            try
            {
                var response = await container.UpsertItemAsync(todoDoc, new PartitionKey(todoDoc.id));

                // Optionally, you can check the status of the response to confirm the upsert operation
                return response.StatusCode == System.Net.HttpStatusCode.Created ||
                       response.StatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (CosmosException ex)
            {
                throw new ApplicationException($"Error in upserting todo: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error in upserting todo", ex);
            }
        }


    }
}
