using OrganizerApi.Cookbook.CookModels;

namespace OrganizerApi.Cookbook.CookServices
{
    public interface ICookBookService
    {

        Task<UserCookBook> GetCookBook(string username);

        UserCookBook PopulateCookBookDemos(string username);
    }
}
