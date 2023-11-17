using OrganizerApi.Cookbook.CookModels;
using OrganizerApi.Cookbook.CookModels.CookbookDTOs.shoppinglist;

namespace OrganizerApi.Cookbook.CookServices
{
    public interface IShoppinglistService
    {
        Task<bool> AddDataToShoppingList(string username, SingleShopList shopList);
        Task<List<ShoppingListRecipeDetails>> CreateShoppingListFromRecipies(string username, List<ShoppingListDetailsDTO> recipiesToUse);
        Task<ShoppingListPageDTO> GetShoppingListData(string username);
        Task<bool> UpdateShoppingListData(string username, ShoppingListPageDTO newShoppingList);
    }
}
