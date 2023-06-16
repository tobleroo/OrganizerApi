using Microsoft.Azure.Cosmos;
using System.ComponentModel;
using Container = Microsoft.Azure.Cosmos.Container;

namespace OrganizerApi.Repository
{
    public class DbConnection
    {
        private readonly string endpointUri;
        private readonly string primaryKey;
        private readonly string databaseId;
        private readonly string containerId;
        private CosmosClient cosmosClient;
        private Database database;
        private Container container;

        public DbConnection(string endpointUri, string primaryKey, string databaseId, string containerId)
        {
            this.endpointUri = endpointUri;
            this.primaryKey = primaryKey;
            this.databaseId = databaseId;
            this.containerId = containerId;
        }

        public async Task<Container> GetContainer()
        {
            if (container == null)
            {
                cosmosClient = new CosmosClient(endpointUri, primaryKey);
                database = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
                //database = cosmosClient.GetDatabase(databaseId);
                if (database == null)
                {
                    await CreateDatabase();
                    database = cosmosClient.GetDatabase(databaseId);
                }

                // Get the container reference
                CreateUsersContainer();
                container = database.GetContainer(containerId);

                if (container == null)
                {
                    var containerProperties = new ContainerProperties(containerId, "id");
                    container = await database.CreateContainerIfNotExistsAsync(containerProperties);
                }
            }
            return container;
        }

        private async Task CreateDatabase()
        {
            database = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
        }

        private void CreateUsersContainer()
        {
            var containerProperties = new ContainerProperties(containerId, "/id");
            container = database.CreateContainerIfNotExistsAsync(containerProperties).GetAwaiter().GetResult();
        }
    }
}
