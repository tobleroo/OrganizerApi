using Microsoft.Azure.Cosmos;
using OrganizerApi.Diary.models;
using OrganizerApi.Diary.models.DiaryDTOs;

namespace OrganizerApi.Diary.Repository
{
    public interface IDiaryRepository
    {
        Task CreateContainerIfNotExistsAsync(Database database, string containerId, int? throughput = null);
        Task<string> FetchPassword(string username);
        Task<UserDiary>? GetDiary(string username);
        Task<string> GetDocumentIdByUsernameAsync(string username);
        Task<ProcessData> PatchNewStory(string diaryId, DiaryPost newPostData);
        Task<bool> UpsertDiary(UserDiary diary);
    }
}
