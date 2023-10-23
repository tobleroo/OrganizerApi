using OrganizerApi.Cookbook.CookModels;

namespace OrganizerApi.Cookbook.CookServices
{
    public interface ICookBookService
    {

        Task<UserCookBook> GetCookBook(string username);

        UserCookBook PopulateCookBookDemos(string username);

        Task<bool> UpdateCookbook(UserCookBook cookbook);

        Task<bool> UpdateShopppingListOfCookbook(UserCookBook cookbook, ShoppingList newShoppingList);
        Task<ShoppingList> FetchShoppingList(string username);
    }
}
