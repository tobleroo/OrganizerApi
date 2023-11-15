using OrganizerApi.Cookbook.CookModels;
using OrganizerApi.Cookbook.CookModels.CookbookDTOs;
using OrganizerApi.Cookbook.CookModels.CookbookDTOs.shoppinglist;

namespace OrganizerApi.Cookbook.CookBookUtils
{
    public static class ShoppingListCreator
    {

        //public static List<ShoppingListRecipeDetails> CreateShoppingList(List<ShoppingListDetailsDTO> wantedRecipies, List<Recipe> userCookbook)
        //{

        //    List<ShoppingListRecipeDetails> shoppingList = new();

        //    foreach (var item in wantedRecipies)
        //    {
        //        foreach (var recipe in userCookbook)
        //        {
        //            if (recipe.RecipeName == item.RecipeName)
        //            {
        //                List<Ingredient> updatedListToNewPortionSize = new List<Ingredient>();

        //                foreach (var ingredient in recipe.Ingredients)
        //                {
        //                    var updatedDataToIngredient = ConvertIngrToNewPortionValue(item.PortionsAmount, ingredient, recipe.Portions);
        //                    updatedListToNewPortionSize.Add(updatedDataToIngredient);
        //                }

        //                var ShoppingListItem = new ShoppingListRecipeDetails()
        //                {
        //                    RecipeName = item.RecipeName,
        //                    Ingredients = updatedListToNewPortionSize
        //                };

        //                shoppingList.Add(ShoppingListItem);
        //            }
        //        }
        //    }

        //    return shoppingList;
        //}

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

        public static UserCookBook AddDateToAdditionalItemLatestUse(UserCookBook cookbook, SingleShopList newShopList)
        {

            if (newShopList.AdditionalItems.Count == 0)
            {
                return cookbook;
            }

            var newDate = CreateNewDate();

            foreach (var item in newShopList.AdditionalItems.Where(item => !cookbook.ShoppingList.AdditionalItems.Contains(item)))
            {
                UpdateOrCreateAdditionalItem(cookbook, item, newDate);
            }

            return cookbook;

        }

        private static void UpdateOrCreateAdditionalItem(UserCookBook cookbook, string item, string newDate)
        {
            var existingItem = cookbook.PreviouslyAddedAdditonalItems.FirstOrDefault(addItem => addItem.Name.Equals(item));

            if (existingItem != null)
            {
                existingItem.DatesWhenShopped.Add(newDate);
            }
            else
            {
                var newAddItem = new AdditionalFoodItem
                {
                    Name = item,
                    DatesWhenShopped = new List<string> { newDate }
                };

                cookbook.PreviouslyAddedAdditonalItems.Add(newAddItem);
            }
        }

        private static string CreateNewDate()
        {
            DateTime currentDate = DateTime.Now;
            return currentDate.ToString("dd/MM/yyyy");
        }

        public static List<string> CheckIfItIsTimeToBuyAgain(List<AdditionalFoodItem> additionalItem)
        {
            List<string> suggestedItemsToBuy = new();

            if(additionalItem.Count > 0) {

                foreach (var addItem in additionalItem)
                {
                    if (SeeIfItemShouldBeBoughtAgain(addItem))
                    {
                        suggestedItemsToBuy.Add(addItem.Name);
                    }
                }
            }
            return suggestedItemsToBuy;
        }

        private static bool SeeIfItemShouldBeBoughtAgain(AdditionalFoodItem additionalItem)
        {

            if (additionalItem.DatesWhenShopped.Count < 3)
            {
                return false;
            }

            List<DateTime> dates = additionalItem.DatesWhenShopped.Select(date => DateTime.Parse(date)).ToList();

            //skip is there to make sure it is n-1 and not get out of bounds exception
            int totalDays = dates.Zip(dates.Skip(1), (date1, date2) => (int)(date2 - date1).TotalDays).Sum();

            int averageDays = totalDays / (additionalItem.DatesWhenShopped.Count - 1);

            DateTime latestDateShopped = dates.Last();
            DateTime todaysDate = DateTime.Now;

            int sinceLastTimeDays = (int)(todaysDate - latestDateShopped).TotalDays;

            return sinceLastTimeDays >= averageDays;
        }

    }
}
