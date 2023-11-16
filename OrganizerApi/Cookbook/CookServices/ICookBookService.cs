using OrganizerApi.Cookbook.CookModels;
using OrganizerApi.Cookbook.CookModels.CookbookDTOs;
using OrganizerApi.Cookbook.CookModels.CookbookDTOs.shoppinglist;

namespace OrganizerApi.Cookbook.CookServices
{
    public interface ICookBookService
    {
        Task<UserCookBook> GetCookBook(string username);

        //UserCookBook PopulateCookBookDemos(string username);

        Task<bool> UpdateCookbook(UserCookBook cookbook);

        //Task<SingleShopList> FetchShoppingList(string username);

        //Task<List<string>> FetchAdditonalItemsFromCosmos(string username);

        Task<List<RecipeOverviewData>> FetchRecipeOverviewData(string username);

        Task<Recipe> GetOneRecipe(string username, string recipeId);

        Task<bool> AddOneRecipeToCookbook(string username, Recipe recipe);

        Task<bool> RemoveOneRecipeFromCookbook(string recipeId, string username);
    }
}
