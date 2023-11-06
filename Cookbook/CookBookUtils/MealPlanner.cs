using OrganizerApi.Cookbook.CookModels;
using OrganizerApi.Cookbook.CookModels.CookbookDTOs;
using OrganizerBlazor.Models;

namespace OrganizerApi.Cookbook.CookBookUtils
{
    public static class MealPlanner
    {

        public static List<Recipe> CreateEasyMealPlan(List<RecipeRequestEasyDTO> desiredTypes, List<Recipe> userCookbook)
        {

            List<Recipe> result = new();

            //run thru all desired recipes.
            foreach (var desiredReqs in desiredTypes)
            {
                //filter for the requirement
                var filteredCookbook = userCookbook
                    .Where(recipe =>
                    recipe.CookTime <= desiredReqs.MaxCookTime &&
                    (desiredReqs.Difficulty == "Any" || recipe.Difficulty.ToString() == desiredReqs.Difficulty) &&
                    (desiredReqs.Category == "Any" || desiredReqs.Category == recipe.RecipeType.ToString()))
                    .ToList();

                //add a random of the ones left in the list
                if (filteredCookbook.Count >= 1)
                {
                    int random = new Random().Next(0, filteredCookbook.Count);
                    result.Add(filteredCookbook[random]);
                }

            }
            return result;
        }

        public static Dictionary<string, List<SpecificRecipeForMealGenDetails>> CreateRecipeListForSepcificGenerator(UserCookBook cookbook)
        {
            List<string> recipeTypesToCheck = new List<string>()
            {
                "Breakfast", "Lunch", "Dinner", "Dessert", "Snack"
            };

            Dictionary<string, List<SpecificRecipeForMealGenDetails>> recipeList = new();

            for (int i = 0; i < recipeTypesToCheck.Count; i++)
            {
                List<SpecificRecipeForMealGenDetails> result = new();
                foreach (var recipeCheck in cookbook.Recipes)
                {
                    if (recipeCheck.RecipeType.ToString() == recipeTypesToCheck[i])
                    {
                        var recippeDetailsDTO = new SpecificRecipeForMealGenDetails()
                        {
                            id = recipeCheck.Guid.ToString(),
                            RecipeName = recipeCheck.RecipeName,
                        };
                        result.Add(recippeDetailsDTO);
                    }
                }
                recipeList.Add(recipeTypesToCheck[i], result);
            }

            return recipeList;

        }

        public static List<Recipe> CreateSpecificMealPlan(List<RecipeRequestSpecificDTO> recipiesWanted, List<Recipe> cookbook)
        {
            return cookbook
                .Where(r => recipiesWanted.Any(req => req.Name == r.RecipeName))
                .ToList();
        }

    }
}
