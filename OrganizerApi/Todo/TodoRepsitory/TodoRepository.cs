using Microsoft.Azure.Cosmos;
using OrganizerApi.Auth.models;
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
                var query = new QueryDefinition("SELECT VALUE c.TodoDoc FROM c WHERE c.Name = @username")
                    .WithParameter("@username", username);

                var iterator = container.GetItemQueryIterator<TodoDocument>(query);
                var response = await iterator.ReadNextAsync();

                var todoDocument = response.FirstOrDefault();
                if (todoDocument == null)
                {
                    // If there is no TodoDocument, create new and return
                    return new TodoDocument() { Owner = username };
                }

                if(todoDocument.Owner == "not set!") todoDocument.Owner = username;

                return todoDocument;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error in todo repository method", ex);
            }
        }

        public async Task<bool> UpsertTodo(string username, TodoDocument todoDoc)
        {
            try
            {
                var query = new QueryDefinition("SELECT * FROM c WHERE c.Name = @username")
                    .WithParameter("@username", username);

                var iterator = container.GetItemQueryIterator<AppUser>(query);
                var response = await iterator.ReadNextAsync();
                var user = response.FirstOrDefault();

                if (user == null)
                {
                    return false; // User not found
                }

                user.TodoDoc = todoDoc;
                var updatedResponse = await container.ReplaceItemAsync(user, user.Id.ToString(), new PartitionKey(user.Id.ToString()));
                return updatedResponse.StatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error in upserting todo", ex);
            }
        }


    }
}
