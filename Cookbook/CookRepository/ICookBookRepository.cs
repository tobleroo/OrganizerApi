using OrganizerApi.Cookbook.CookModels;

namespace OrganizerApi.Cookbook.CookRepository
{
    public interface ICookBookRepository
    {

        Task<UserCookBook> GetCookBook(string id);

        Task<UserCookBook> SaveNewCookBook(UserCookBook cookbook);
        Task<bool> UpdateCookBook(UserCookBook cookbook);

        Task<ShoppingList> GetShoppingList(string username);
    }
}
