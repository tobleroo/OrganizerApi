using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizerApi.Cookbook.CookModels.CookbookDTOs.shoppinglist;
using OrganizerApi.Cookbook.CookModels;
using OrganizerApi.Cookbook.CookRepository;
using OrganizerApi.Cookbook.CookServices;
using System.Security.Claims;
using OrganizerBlazor.Models;
using OrganizerApi.Cookbook.CookBookUtils;

namespace OrganizerApi.Cookbook.CookbookControllers
{

    [ApiController]
    [Route("shoppinglist")]
    [Authorize]
    public class ShoppingListController : ControllerBase
    {
        private readonly ICookBookRepository _cookbookRepository;
        private readonly ICookBookService _cookBookService;
        public ShoppingListController(ICookBookRepository cookBookRepository, ICookBookService cookBookService)
        {
            _cookbookRepository = cookBookRepository;
            _cookBookService = cookBookService;
        }

        [HttpPost("create-shoppinglist")]
        public async Task<ActionResult<List<ShoppingListRecipeDetails>>> CreateShoppingList([FromBody] List<ShoppingListDetailsDTO> recipiesToUse)
        {
            var name = User.FindFirstValue(ClaimTypes.Name);
            var cookBook = await _cookbookRepository.GetCookBook(name);
            if (cookBook == null)
            {
                return NotFound();
            }

            var finishedShoppingList = ShoppingListCreator.CreateShoppingList(recipiesToUse, cookBook.Recipes);
            return Ok(finishedShoppingList);
        }

        [HttpPost("add-to-shoppinglist")]
        public async Task<IActionResult> AddToShoppinglist([FromBody] SingleShopList shopList)
        {
            var name = User.FindFirstValue(ClaimTypes.Name);
            var cookBook = await _cookBookService.GetCookBook(name);

            //run through additional items and check dates
            _cookBookService.RecommendAdditionalItems(cookBook, shopList);

            //add recipes to current shoppinglist
            bool successfullyAddedRecipes = await _cookBookService.AddRecipesToShoppingList(cookBook, shopList);


            return successfullyAddedRecipes ? Ok() : BadRequest();
        }

        [HttpGet("get-shoppinglist")]
        public async Task<ShoppingListPageDTO> RecieveShoppingList()
        {
            var name = User.FindFirstValue(ClaimTypes.Name);
            var list = await _cookBookService.FetchShoppingList(name);
            // var additonalItems = await _cookBookService.FetchAdditonalItemsFromCosmos(name);

            var cookbook = await _cookBookService.GetCookBook(name);
            var recommededAddItems = ShoppingListCreator.CheckIfItIsTimeToBuyAgain(cookbook.PreviouslyAddedAdditonalItems);
            ShoppingListPageDTO shoppingPageData = new ShoppingListPageDTO()
            {
                SingleShopList = list,
                AdditionalItems = list.AdditionalItems,
                RecommendedAdditionalItems = recommededAddItems
            };
            return shoppingPageData;
        }

        [HttpPost("update-shoppinglist")]
        public async Task<IActionResult> UpdateShopingListItems(ShoppingListPageDTO updatedList)
        {
            var name = User.FindFirstValue(ClaimTypes.Name);
            var success = await _cookBookService.UpdateShoppingListOfCookbook(name, updatedList);
            return Ok(success);
        }
    }
}
