using OrganizerApi.Cookbook.CookBookUtils;
using OrganizerApi.Cookbook.CookModels;
using OrganizerApi.Cookbook.CookModels.CookbookDTOs.shoppinglist;
using OrganizerApi.Cookbook.CookRepository;

namespace OrganizerApi.Cookbook.CookServices
{
    public class ShoppinglistService : IShoppinglistService
    {

        private readonly ICookBookRepository _cookbookRepository;

        public ShoppinglistService(ICookBookRepository cookbookRepository)
        {
            _cookbookRepository = cookbookRepository;
        }

        public async Task<List<ShoppingListRecipeDetails>> CreateShoppingListFromRecipies(string username, List<ShoppingListDetailsDTO> recipiesToUse)
        {
            var cookBook = await _cookbookRepository.GetCookBook(username);

            var recipesCreated = ShoppingListCreator.CreateShoppingList(recipiesToUse, cookBook.Recipes);

            return recipesCreated;
        }

        public async Task<bool> AddDataToShoppingList(string username, SingleShopList shopList)
        {
            var cookBook = await _cookbookRepository.GetCookBook(username);

            //this has to be before addRnges below, otherwise it cant separate existing to new items added
            cookBook = ShoppingListCreator.AddItemToAdditionalList(cookBook, shopList);

            cookBook.ShoppingList.SingleShopListRecipes.AddRange(shopList.SingleShopListRecipes);
            cookBook.ShoppingList.AdditionalItems.AddRange(shopList.AdditionalItems);


            return await _cookbookRepository.UpdateCookBook(cookBook);
        }

        public async Task<ShoppingListPageDTO> GetShoppingListData(string username)
        {
            var cookBook = await _cookbookRepository.GetCookBook(username);

            var recommededAddItems = ShoppingListCreator.CheckIfItIsTimeToBuyAgain(cookBook.PreviouslyAddedAdditonalItems);
            ShoppingListPageDTO shoppingPageData = new ShoppingListPageDTO()
            {
                SingleShopList = cookBook.ShoppingList,
                //this is separated for the blazor components, can change later
                AdditionalItems = cookBook.GetPreviouslyAddedItemsAsListOfStrings(),
                RecommendedAdditionalItems = recommededAddItems
            };
            return shoppingPageData;
        }
        public async Task<bool> UpdateShoppingListData(string username, ShoppingListPageDTO newShoppingList)
        {
            var cookbook = await _cookbookRepository.GetCookBook(username);

            //do the additional item stuffs before replacing the shoppinglist
            cookbook = ShoppingListCreator.AddItemToAdditionalList(cookbook, newShoppingList.SingleShopList);

            //remove the items that has been completed

            //remove from recipe ingredients
            foreach (var recipe in newShoppingList.SingleShopList.SingleShopListRecipes)
            {
                recipe.Ingredients.RemoveAll(ingredient => ingredient.Finished);
            }

            //check if any recipe contains 0 recipies, if yes, remove
            newShoppingList.SingleShopList.SingleShopListRecipes = newShoppingList.SingleShopList.SingleShopListRecipes
                                                                        .Where(recipe => recipe.Ingredients.Count > 0)
                                                                        .ToList();

            newShoppingList.SingleShopList.AdditionalItems = newShoppingList.SingleShopList.AdditionalItems
                                                                .Where(extra => extra.Completed != true)
                                                                .ToList();


            cookbook.ShoppingList = newShoppingList.SingleShopList;

            return await _cookbookRepository.UpdateCookBook(cookbook);
        }
    }
}
