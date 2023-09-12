using OrganizerApi.Cookbook.CookModels;

namespace OrganizerApi.Cookbook.CookServices
{
    public static class MealPlanner
    {
        public static List<Recipe> GetRecipesForWeek(UserCookBook cookBook)
        {
            var recipes = new List<Recipe>();
            var random = new Random();
            var recipeCount = cookBook.Recipes.Count;
            for (int i = 0; i < recipeCount; i++)
            {
                var recipeIndex = random.Next(recipeCount);
                recipes.Add(cookBook.Recipes[recipeIndex]);
            }
            return recipes;
        }
    }
}
