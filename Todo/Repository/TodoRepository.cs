using Microsoft.Azure.Cosmos;
using OrganizerApi.Auth.Repository;
using OrganizerApi.Todo.models;
using System.Reflection.Metadata;
using System.Xml;
using System.Xml.Linq;

namespace OrganizerApi.Todo.Repository
{
    public class TodoRepository : ITodoRepository
    {

        private Container container;

        public TodoRepository() => InitializeContainerAsync().Wait();

        private async Task InitializeContainerAsync()
        {
            var conn = new DbConnection("https://localhost:8081", "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
                "Organizer", "todo");
            container = await conn.GetContainer(); // Await the Task<Container> object to get the Container
        }


        public void AddTaskCategory(string categoryName)
        {
            
            
        }

        public void AddTaskToCategory(string categoryName, string taskName, int TimeToDo, int TimeToComplete, int TimesCompleted, string LastTimeCompleted)
        {
            throw new NotImplementedException();
        }

        public void DeleteTaskCategory(string categoryName)
        {
            throw new NotImplementedException();
        }

        public void DeleteTaskFromCategory(string categoryName, string taskName)
        {
            throw new NotImplementedException();
        }

        public async Task<TodoDocument?> GetUserTodoData(string username)
        {
            TodoDocument? foundItem = null;

            // Create query in Cosmos DB to get data by username as partition key
            var sqlQueryText = "SELECT * FROM c WHERE c.Owner = @Owner";
            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText).WithParameter("@Owner", username);

            using (FeedIterator<TodoDocument> feedIterator = container.GetItemQueryIterator<TodoDocument>(queryDefinition))
            {
                if (feedIterator.HasMoreResults)
                {
                    FeedResponse<TodoDocument> response = await feedIterator.ReadNextAsync();
                    foundItem = response.FirstOrDefault(); // Assuming there's at most one item
                }
            }

            return foundItem;
        }

        public async Task<TodoDocument?> CreateTodoData(TodoDocument todoDb)
        {
            try
            {
                ItemResponse<TodoDocument> response = await container.CreateItemAsync(todoDb, new PartitionKey(todoDb.Id.ToString()));
                return response; // Data saved successfully
            }
            catch (Exception ex)
            {
                // Handle any exceptions here

                return null;
            }
        }
    }
}
