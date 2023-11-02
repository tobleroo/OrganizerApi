using OrganizerApi.Cookbook.CookModels;
using OrganizerApi.Cookbook.CookModels.CookbookDTOs;
using OrganizerApi.Cookbook.CookModels.CookbookDTOs.shoppinglist;

namespace OrganizerApi.Cookbook.CookServices
{
    public interface ICookBookService
    {

        Task<UserCookBook> GetCookBook(string username);

        UserCookBook PopulateCookBookDemos(string username);

        Task<bool> UpdateCookbook(UserCookBook cookbook);

        Task<bool> UpdateShoppingListOfCookbook(string username, SingleShopList newShoppingList);
        Task<SingleShopList> FetchShoppingList(string username);

        Task<bool> AddNewAdditonalItemsToCookbook(string username, List<string> additionalItemsSaved, List<string> newItemsTosave);

        Task<bool> AddRecipesToShoppingList(UserCookBook cookbook,SingleShopList shoplistDetails);
    }
}
