using OrganizerApi.Cookbook.CookModels;
using OrganizerApi.Cookbook.CookModels.CookbookDTOs;

namespace OrganizerApi.Cookbook.CookServices
{
    public interface ICookBookService
    {

        Task<UserCookBook> GetCookBook(string username);

        UserCookBook PopulateCookBookDemos(string username);

        Task<bool> UpdateCookbook(UserCookBook cookbook);

        Task<bool> UpdateShopppingListOfCookbook(UserCookBook cookbook, ShoppingList newShoppingList);
        Task<ShoppingListALLItems> FetchShoppingList(string username);

        Task<bool> AddNewAdditonalItemsToCookbook(string username, List<string> additionalItemsSaved, List<string> newItemsTosave);
    }
}
