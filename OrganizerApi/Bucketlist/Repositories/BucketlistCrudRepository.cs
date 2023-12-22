using Microsoft.Azure.Cosmos;
using OrganizerApi.Auth.models;
using OrganizerApi.Bucketlist.Models;
using OrganizerBlazor.Todo.Models;

namespace OrganizerApi.Bucketlist.Repositories
{
    public class BucketlistCrudRepository : IBucketlistCrudRepository
    {

        private Container container;

        public BucketlistCrudRepository(CosmosClient cosmosClient, string databaseId, string containerId, int? throughput = null)
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

        public async Task<BucketlistDocument> GetBucketlist(string username)
        {
            try
            {
                var query = new QueryDefinition("SELECT VALUE c.Bucketlist FROM c WHERE c.Name = @username")
                    .WithParameter("@username", username);

                var iterator = container.GetItemQueryIterator<BucketlistDocument>(query);
                var response = await iterator.ReadNextAsync();

                var bucketDocument = response.FirstOrDefault();
                if (bucketDocument == null)
                {
                    // If there is no TodoDocument, create new and return
                    return new BucketlistDocument() { Owner = username };
                }

                if (bucketDocument.Owner == "not set!") bucketDocument.Owner = username;

                return bucketDocument;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error in todo repository method", ex);
            }
        }

        public async Task<bool> UpdateBucketlist(string username, BucketlistDocument bucketlist)
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

                user.Bucketlist = bucketlist;
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
