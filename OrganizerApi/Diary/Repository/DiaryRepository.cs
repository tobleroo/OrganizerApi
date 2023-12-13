using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Hosting;
using OrganizerApi.Diary.models;
using OrganizerApi.Diary.models.DiaryDTOs;
using System.Dynamic;


namespace OrganizerApi.Diary.Repository
{
    public class DiaryRepository : IDiaryRepository
    {
        private Container container;

        public DiaryRepository(CosmosClient cosmosClient, string databaseId, string containerId, int? throughput = null)
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

        public async Task<UserDiary>? GetDiary(string username)
        {
            try
            {
                var sqlQueryText = $"SELECT * FROM c WHERE c.OwnerUsername = '{username}'";
                QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
                FeedIterator<UserDiary> queryResultSetIterator = container.GetItemQueryIterator<UserDiary>(queryDefinition);

                while (queryResultSetIterator.HasMoreResults)
                {
                    FeedResponse<UserDiary> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                    foreach (UserDiary diary in currentResultSet)
                    {
                        return diary;
                    }
                }

                //if there is no user cookbook, create new and return
                return new UserDiary()
                {
                    OwnerUsername = username
                };
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error in diary repository method", ex);
            }

        }

        public async Task<bool> UpsertDiary(UserDiary diary)
        {
            try
            {
                var response = await container.UpsertItemAsync(diary, new PartitionKey(diary.id));

                // Optionally, you can check the status of the response to confirm the upsert operation
                return response.StatusCode == System.Net.HttpStatusCode.Created ||
                       response.StatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (CosmosException ex)
            {
                throw new ApplicationException($"Error in upserting diary: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error in upserting diary", ex);
            }
        }

        public async Task<string> FetchPassword(string username)
        {
            var query = new QueryDefinition("SELECT c.DiaryPassword FROM c WHERE c.OwnerUsername = @diaryUsername")
            .WithParameter("@diaryUsername", username);

            var iterator = container.GetItemQueryIterator<dynamic>(query);

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                foreach (var item in response)
                {
                    return item.DiaryPassword;
                }
            }

            return string.Empty;
        }

        public async Task<string> GetDocumentIdByUsernameAsync(string username)
        {
            var query = new QueryDefinition("SELECT c.id FROM c WHERE c.OwnerUsername = @username")
                        .WithParameter("@username", username);

            using (FeedIterator<dynamic> feedIterator = container.GetItemQueryIterator<dynamic>(query))
            {
                while (feedIterator.HasMoreResults)
                {
                    FeedResponse<dynamic> response = await feedIterator.ReadNextAsync();
                    foreach (var item in response)
                    {
                        return item.id; // Assuming username is unique and will return only one document
                    }
                }
            }

            return null; // Return null or appropriate value if no document is found
        }

        public async Task<ProcessData> PatchNewStory(string diaryId, DiaryPost newPostData)
        {

            var patchOperations = new List<PatchOperation>
            {
                PatchOperation.Add("/Posts/-", newPostData)
            };

            try
            {
                await container.PatchItemAsync<dynamic>(diaryId, new PartitionKey(diaryId), patchOperations);
                return new ProcessData() { IsValid = true, Message = "successfully added story!" };
            }
            catch (CosmosException ex)
            {
                // Handle exceptions (e.g., document not found, etc.)
                return new ProcessData() { IsValid = false, Message = "something went wrong -> " + ex.Message};
            }

        }

        public async Task<DiaryPost> GetOnePost(string username, string Id)
        {
            string queryString = $"SELECT * FROM c JOIN p IN c.Posts WHERE p.Id = '{Id}'";
            QueryDefinition queryDefinition = new QueryDefinition(queryString);
            FeedIterator<DiaryPost> queryResultSetIterator = container.GetItemQueryIterator<DiaryPost>(queryDefinition);
            List<DiaryPost> posts = new List<DiaryPost>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<DiaryPost> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (DiaryPost post in currentResultSet)
                {
                    posts.Add(post);
                }
            }

            return posts.FirstOrDefault();
        }
    }
}
