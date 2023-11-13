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

        public static UserCookBook AddDateToAdditionalItemLatestUse(UserCookBook cookbook, SingleShopList newShopList)
        {
            
            //go through cookbooks current shoppinglist to see if items from newshoplist doesnt exist already
            if(newShopList.AdditionalItems.Count > 0)
            {
                foreach(var item in newShopList.AdditionalItems)
                {
                    //if item does not already exist
                    if (!cookbook.ShoppingList.AdditionalItems.Contains(item))
                    {
                        bool doesExists = false;
                        //create new date string
                        var newDate = CreateNewDate();

                        foreach(var addItem in cookbook.PreviouslyAddedAdditonalItems)
                        {
                            if (addItem.Name.Equals(item))
                            {
                                addItem.DatesWhenShopped.Add(newDate);
                                doesExists = true;
                                break;
                            }
                        }

                        //if still false, create new additem
                        if(doesExists == false)
                        {
                            AdditionalFoodItem newAddITem = new AdditionalFoodItem()
                            {
                                Name = item,
                            };

                            newAddITem.DatesWhenShopped.Add(newDate);
                            cookbook.PreviouslyAddedAdditonalItems.Add(newAddITem);
                        }

                    }
                }
            }
            return cookbook;
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

            List<DateTime> dates = new List<DateTime>();
            int totalDays = 0;

            if( additionalItem.DatesWhenShopped.Count > 2 ) { 
                for (int i = 0; i < additionalItem.DatesWhenShopped.Count; i++)
                {
                    dates.Add(DateTime.Parse(additionalItem.DatesWhenShopped[i]));
                }

                //compare diff between each date 

                for(int j = dates.Count - 1; j >= 1; j--) {
                    var timeSpan = dates[j - 1].Subtract(dates[j]);
                    totalDays += timeSpan.Days;
                }

                //divide total days by how many times item has been shopped.
                int averageDays = totalDays / additionalItem.DatesWhenShopped.Count;

                //check if its been more than averagedays since last time
                //get latest date
                var latestDateShopped = dates[dates.Count - 1];
                //get todays date and subtract to latest to get diff
                var todaysDate = DateTime.Now;

                var amountDaysSinceLastTime = todaysDate.Subtract(latestDateShopped);

                int sinceLastTimeDays =amountDaysSinceLastTime.Days;

                if(sinceLastTimeDays >= averageDays)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
