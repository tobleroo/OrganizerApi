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

        public static void AddDateToAdditionalItemLatestUse(UserCookBook cookbook, SingleShopList newShopList)
        {
            //go through all shoppinglists in cookbook and find the right one. 
            SingleShopList? ShopListThing = null;

            //find the right shoppinglist
            foreach (var cookbookShoppingList in cookbook.ShoppingLists) {
                if(cookbookShoppingList.ListName == newShopList.ListName)
                {
                    ShopListThing = cookbookShoppingList;
                    break;
                }            
            }

            //go through the old shoplist and see if any of the new things arent there already
            foreach(var newAdditionalItem in newShopList.AdditionalItems)
            {
                if (!ShopListThing.AdditionalItems.Contains(newAdditionalItem))
                {
                    //check if item exists in cookbook additional items list
                    var itemAlreadyExists = cookbook.PreviouslyAddedAdditonalItems.FirstOrDefault(addITem => addITem.Name == newAdditionalItem);
                    
                    if(itemAlreadyExists != null)
                    {
                        //add date to the object
                        itemAlreadyExists.DatesWhenShopped.Add( CreateNewDate() );
                    }
                    else
                    {
                        //creat new addItem object and add to list with a current date
                        AdditionalFoodItem addItemNew = new()
                        {
                            Name = newAdditionalItem,
                        };

                        addItemNew.DatesWhenShopped.Add( CreateNewDate() );

                        cookbook.PreviouslyAddedAdditonalItems.Add( addItemNew );
                    }
                }
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

            //run through each item in the list
            foreach (var addItem in additionalItem)
            {
                if (SeeIfItemShouldBeBoughtAgain(addItem))
                {
                    suggestedItemsToBuy.Add(addItem.Name);
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
