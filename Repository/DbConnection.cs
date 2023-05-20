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

        public Container GetContainer()
        {
            if (container == null)
            {
                cosmosClient = new CosmosClient(endpointUri, primaryKey);
                database = cosmosClient.GetDatabase(databaseId);
                container = database.GetContainer(containerId);
            }
            return container;
        }
    }
}
