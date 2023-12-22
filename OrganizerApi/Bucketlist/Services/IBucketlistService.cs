using OrganizerApi.Bucketlist.Models;
using OrganizerApi.Diary.models.DiaryDTOs;

namespace OrganizerApi.Bucketlist.Services
{
    public interface IBucketlistService
    {
        Task<BucketlistDocument> GetBucketlist(string username);
        Task<ProcessData> SaveNewAchievement(string name, Achievement achievement);
    }
}
