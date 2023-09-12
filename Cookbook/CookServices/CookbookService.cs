using OrganizerApi.Cookbook.CookModels;
using OrganizerApi.Cookbook.CookRepository;

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
                return await _cookbookRepository.SaveNewCookBook(PopulateCookBookDemos(username));
            }
            return cookBook;
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

            var newShoppingLisr = new ShoppingList { ListName = "veckohandel" };
            newShoppingLisr.Ingredients.Add(ingredient1);
            newShoppingLisr.Ingredients.Add(ingredient2);
            newCookBook.ShoppingList.Add(newShoppingLisr);
            return newCookBook;
        }

        public async Task<bool> UpdateCookbook(UserCookBook cookbook)
        {
            return await _cookbookRepository.UpdateCookBook(cookbook);
        }
    }
}
