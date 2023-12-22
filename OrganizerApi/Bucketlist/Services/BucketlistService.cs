using OrganizerApi.Bucketlist.Models;
using OrganizerApi.Bucketlist.Repositories;
using OrganizerApi.Diary.models.DiaryDTOs;

namespace OrganizerApi.Bucketlist.Services
{
    public class BucketlistService : IBucketlistService
    {

        private readonly IBucketlistCrudRepository _bucketlistCrudRepository;

        public BucketlistService(IBucketlistCrudRepository bucketlistCrudRepository) {
            _bucketlistCrudRepository = bucketlistCrudRepository;
        }

        public async Task<BucketlistDocument> GetBucketlist(string username)
        {
            
            return await _bucketlistCrudRepository.GetBucketlist(username);
        }

        public async Task<ProcessData> SaveNewAchievement(string name, Achievement ach)
        {
            //get user doc
            var bucketlist = await _bucketlistCrudRepository.GetBucketlist(name);
            //check if id already exists , if achievement is new or an updated one
            int existingIndex = bucketlist.Achievements.FindIndex(achievement => achievement.id == ach.id);
            if (existingIndex != -1)
            {
                // Replace if ID already exists
                bucketlist.Achievements[existingIndex] = ach;
            }
            else bucketlist.Achievements.Add(ach);

            //now update user with new bucketlist data 
            var success = await _bucketlistCrudRepository.UpdateBucketlist(name, bucketlist);

            return new ProcessData() { IsValid = success };
        }
    }
}
