using OrganizerApi.Cookbook.CookModels;
using OrganizerApi.Cookbook.CookModels.CookbookDTOs;
using OrganizerApi.Cookbook.CookModels.CookbookDTOs.shoppinglist;

namespace OrganizerApi.Cookbook.CookBookUtils
{
    public static class ShoppingListCreator
    {

        public static List<ShoppingListRecipeDetails> CreateShoppingList(List<ShoppingListDetailsDTO> wantedRecipies, List<Recipe> userCookbook)
        {

            List<ShoppingListRecipeDetails> shoppingList = new();

            foreach (var item in wantedRecipies)
            {
                foreach (var recipe in userCookbook)
                {
                    if (recipe.RecipeName == item.RecipeName)
                    {
                        List<Ingredient> updatedListToNewPortionSize = new List<Ingredient>();

                        foreach (var ingredient in recipe.Ingredients)
                        {
                            var updatedDataToIngredient = ConvertIngrToNewPortionValue(item.PortionsAmount, ingredient, recipe.Portions);
                            updatedListToNewPortionSize.Add(updatedDataToIngredient);
                        }

                        var ShoppingListItem = new ShoppingListRecipeDetails()
                        {
                            RecipeName = item.RecipeName,
                            Ingredients = updatedListToNewPortionSize
                        };

                        shoppingList.Add(ShoppingListItem);
                    }
                }
            }

            return shoppingList;
        }

        private static Ingredient ConvertIngrToNewPortionValue(int portionsWanted, Ingredient originalIngredient, int originalPortions)
        {
            //divide amount by original then multiply by new portions wanted
            var newPortionsQuant = originalIngredient.Quantity / originalPortions * portionsWanted;

            return new Ingredient()
            {
                Name = originalIngredient.Name,
                Quantity = newPortionsQuant,
                Unit = originalIngredient.Unit,
            };
        }

    }
}
