using OrganizerApi.Cookbook.CookModels;
using OrganizerApi.Cookbook.CookModels.CookbookDTOs;

namespace OrganizerApi.Cookbook.CookRepository
{
    public interface ICookBookRepository
    {

        Task<UserCookBook> GetCookBook(string id);

        Task<UserCookBook> SaveNewCookBook(UserCookBook cookbook);
        Task<bool> UpdateCookBook(UserCookBook cookbook);

        Task<ShoppingListALLItems?> GetShoppingList(string username);

        Task<bool> UpsertAdditionalItemsShoppingList(string username, List<string> shoppinglistToUpdate);
    }
}
