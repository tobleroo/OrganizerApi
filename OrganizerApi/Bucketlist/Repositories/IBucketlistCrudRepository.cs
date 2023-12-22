using OrganizerApi.Bucketlist.Models;

namespace OrganizerApi.Bucketlist.Repositories
{
    public interface IBucketlistCrudRepository
    {
        Task<bool> UpdateBucketlist(string username, BucketlistDocument bucketlist);
        Task<BucketlistDocument> GetBucketlist(string username);

    }
}
