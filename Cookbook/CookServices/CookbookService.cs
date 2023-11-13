using OrganizerApi.Cookbook.CookBookUtils;
using OrganizerApi.Cookbook.CookModels;
using OrganizerApi.Cookbook.CookModels.CookbookDTOs;
using OrganizerApi.Cookbook.CookModels.CookbookDTOs.shoppinglist;
using OrganizerApi.Cookbook.CookRepository;
using System.Drawing.Text;

namespace OrganizerApi.Cookbook.CookServices
{
    public class CookbookService : ICookBookService
    {

        //get cookbook repository
        private readonly ICookBookRepository _cookbookRepository;
        public CookbookService(ICookBookRepository cookBookRepository) {
            _cookbookRepository = cookBookRepository;
        }

        public void AddRecipe(Recipe recipe)
        { }

        public async Task<UserCookBook> GetCookBook(string username)
        {
            var cookBook = await _cookbookRepository.GetCookBook(username);
            if (cookBook == null)
            {
                //return await _cookbookRepository.SaveNewCookBook(PopulateCookBookDemos(username));
            }
            return cookBook;
        }

        public async Task<bool> AddRecipesToShoppingList(UserCookBook cookbook,SingleShopList shoplistDetails)
        {
            //go through the cookbook 
            var allShoppingLists = cookbook.ShoppingLists;

            var originalShoplist = allShoppingLists.FirstOrDefault(shopplingList => shopplingList.ListName == shoplistDetails.ListName);

            if(originalShoplist == null) cookbook.ShoppingLists.Add(shoplistDetails);
                else originalShoplist.SingleShopListRecipes.AddRange(shoplistDetails.SingleShopListRecipes);

            return await UpdateCookbook(cookbook);

        }

        public async Task<bool> UpdateShoppingListOfCookbook(string username, ShoppingListPageDTO newShoppingList)
        {
            var cookbook = await _cookbookRepository.GetCookBook(username);

            //do the additional item stuffs before replacing the shoppinglist
            ShoppingListCreator.AddDateToAdditionalItemLatestUse(cookbook, newShoppingList.SingleShopList);
            var recommendedAddItems = ShoppingListCreator.CheckIfItIsTimeToBuyAgain(cookbook.PreviouslyAddedAdditonalItems);


            cookbook.ShoppingLists.RemoveAll(name => name.ListName == newShoppingList.SingleShopList.ListName);
            cookbook.ShoppingLists.Add(newShoppingList.SingleShopList);

            if (cookbook.ShoppingLists.Contains(newShoppingList.SingleShopList))
            {
                await _cookbookRepository.UpdateCookBook(cookbook);
                return true;
            }
            return false;
        }

        public UserCookBook PopulateCookBookDemos(string username)
        {
            var newCookBook = new UserCookBook { OwnerUsername = username};

            //create 2 ingredients
            var ingredient1 = new Ingredient { Name = "Chicken Breast", Quantity = 2, Unit = "lbs" };
            var ingredient2 = new Ingredient { Name = "Pasta", Quantity = 1, Unit = "lbs" };

            var recipe = new Recipe { RecipeName = "Chicken Parmesan" };
            //set recipe ingredient list with created ingredients
            recipe.Ingredients.Add(ingredient1);
            recipe.Ingredients.Add(ingredient2);

            newCookBook.Recipes.Add(recipe);
            // create more recipes
            var recipe2 = new Recipe { RecipeName = "Chicken Alfredo" };
            recipe2.Ingredients.Add(ingredient1);
            recipe2.Ingredients.Add(ingredient2);
            newCookBook.Recipes.Add(recipe2);

            var recipe3 = new Recipe { RecipeName = "carbonara" };
            recipe3.Ingredients.Add(ingredient1);
            recipe3.Ingredients.Add(ingredient2);
            newCookBook.Recipes.Add(recipe3);

            var recipe4 = new Recipe { RecipeName = "meatloaf" };
            recipe4.Ingredients.Add(ingredient1);
            recipe4.Ingredients.Add(ingredient2);
            newCookBook.Recipes.Add(recipe4);

            return newCookBook;
        }

        public async Task<bool> UpdateCookbook(UserCookBook cookbook)
        {
            return await _cookbookRepository.UpdateCookBook(cookbook);
        }

        public async Task<SingleShopList> FetchShoppingList(string username)
        {
            var cookbookId = await _cookbookRepository.FetchUserCookbookId(username);
            return await _cookbookRepository.GetShoppingList(username, cookbookId);
        }

        //public async Task<List<string>> FetchAdditonalItemsFromCosmos(string username)
        //{
        //    var cookbookId = await _cookbookRepository.FetchUserCookbookId(username);
        //    return await _cookbookRepository.FetchAdditionalItemsFromShoppingLists(username, cookbookId);
        //}

        public List<string> RecommendAdditionalItems(UserCookBook cookbook, SingleShopList shoplist)
        {
            List<string> items = new List<string>();
            ShoppingListCreator.AddDateToAdditionalItemLatestUse(cookbook, shoplist);

            return items;
        }

        public async Task<List<RecipeOverviewData>> FetchRecipeOverviewData(string username)
        {
            //do the repo directly
            return await _cookbookRepository.FetchRecipiesOverview(username);
        }

        public async Task<Recipe> GetOneRecipe(string username, string recipeId)
        {
            //do the repo directly 
            return await _cookbookRepository.FetchOneRecipe(username, recipeId);
        }

        public async Task<bool> AddOneRecipeToCookbook(string username, Recipe recipe)
        {
            var cookbook = await _cookbookRepository.GetCookBook(username);

            //check if recipe id exists in cookbook already 
            // Find the index of the existing recipe
            int index = cookbook.Recipes.FindIndex(r => r.Guid == recipe.Guid);

            if (index != -1)
            {
                // Replace the existing recipe with the new one
                cookbook.Recipes[index] = recipe;
            }else cookbook.Recipes.Add(recipe);

            return await _cookbookRepository.UpdateCookBook(cookbook);

        }

        public async Task<bool> RemoveOneRecipeFromCookbook(string recipeId, string username)
        {
            var cookbook = await _cookbookRepository.GetCookBook(username);
            cookbook.Recipes.RemoveAll(item => item.Guid.ToString().Equals(recipeId));
            return await _cookbookRepository.UpdateCookBook(cookbook);
        }
    }
}
