using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Hosting;
using OrganizerApi.Auth.models;
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

        public async Task<UserDiary> GetDiary(string username)
        {
            try
            {
                var query = new QueryDefinition("SELECT VALUE c.Diary FROM c WHERE c.Name = @username")
            .WithParameter("@username", username);

                var iterator = container.GetItemQueryIterator<UserDiary>(query);
                var response = await iterator.ReadNextAsync();

                return response.FirstOrDefault() ?? new UserDiary { OwnerUsername = username };
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new UserDiary { OwnerUsername = username };
            }
        }

        public async Task<bool> UpdateDiary(string username, UserDiary diary)
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
                    return false;
                }

                user.Diary = diary;
                await container.ReplaceItemAsync(user, user.Id.ToString(), new PartitionKey(user.Id.ToString()));
                return true;
            }
            catch (CosmosException)
            {
                throw;
            }
        }

        public async Task<string> FetchPassword(string username)
        {
            var query = new QueryDefinition("SELECT c.Diary.DiaryPassword FROM c WHERE c.Name = @username")
                .WithParameter("@username", username);

            var iterator = container.GetItemQueryIterator<dynamic>(query);

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                foreach (var item in response)
                {
                    // Since the DiaryPassword is directly selected, it should be directly accessible.
                    if (item.DiaryPassword != null)
                    {
                        return item.DiaryPassword.ToString();
                    }
                }
            }

            return string.Empty; // Return empty string if not found.
        }

        public async Task<string> GetDocumentIdByUsernameAsync(string username)
        {
            var query = new QueryDefinition("SELECT c.Diary.id FROM c WHERE c.Name = @username")
                        .WithParameter("@username", username);

            using (FeedIterator<dynamic> feedIterator = container.GetItemQueryIterator<dynamic>(query))
            {
                while (feedIterator.HasMoreResults)
                {
                    FeedResponse<dynamic> response = await feedIterator.ReadNextAsync();
                    foreach (var item in response)
                    {
                        // Extract and return the id of the Diary object
                        return item.id as string; // Ensure type casting to string
                    }
                }
            }

            return null; // Return null if no Diary id is found
        }

        //public async Task<ProcessData> PatchNewStory(string diaryId, DiaryPost newPostData)
        //{

        //    var patchOperations = new List<PatchOperation>
        //    {
        //        PatchOperation.Add("/Posts/-", newPostData)
        //    };

        //    try
        //    {
        //        await container.PatchItemAsync<dynamic>(diaryId, new PartitionKey(diaryId), patchOperations);
        //        return new ProcessData() { IsValid = true, Message = "successfully added story!" };
        //    }
        //    catch (CosmosException ex)
        //    {
        //        // Handle exceptions (e.g., document not found, etc.)
        //        return new ProcessData() { IsValid = false, Message = "something went wrong -> " + ex.Message};
        //    }

        //}

        public async Task<DiaryPost> GetOnePost(string username, string postId)
        {
            string queryString = $"SELECT VALUE p FROM c JOIN p IN c.Diary.Posts WHERE c.Name = @username AND p.Id = @postId";
            QueryDefinition queryDefinition = new QueryDefinition(queryString)
                .WithParameter("@username", username)
                .WithParameter("@postId", postId);

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
