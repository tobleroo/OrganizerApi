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

            var newShoppingLisr = new ShoppingList { ListName = "veckohandel" };
            newCookBook.ShoppingList.Add(newShoppingLisr);
            return newCookBook;
        }
    }
}
