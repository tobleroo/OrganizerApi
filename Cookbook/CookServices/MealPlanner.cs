using OrganizerApi.Cookbook.CookModels;
using OrganizerBlazor.Models;

namespace OrganizerApi.Cookbook.CookServices
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
                    (recipe.CookTime <= desiredReqs.MaxCookTime) &&
                    (desiredReqs.Difficulty ==  "Any" || recipe.Difficulty.ToString() == desiredReqs.Difficulty) &&
                    (desiredReqs.Category == "Any" || desiredReqs.Category == recipe.RecipeType.ToString()))
                    .ToList();

                //add a random of the ones left in the list
                
                if(filteredCookbook.Count >= 1) {
                    int random = new Random().Next(0, filteredCookbook.Count);
                    result.Add(filteredCookbook[random]);
                }

            }

            return result;
        }

    }
}
