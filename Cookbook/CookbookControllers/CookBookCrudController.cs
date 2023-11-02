using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizerApi.Cookbook.CookModels;
using OrganizerApi.Cookbook.CookModels.CookbookDTOs;
using OrganizerApi.Cookbook.CookModels.CookbookDTOs.shoppinglist;
using OrganizerApi.Cookbook.CookServices;
using System.Security.Claims;

namespace OrganizerApi.Cookbook.CookCrudControllers
{

    [Route("/cookbook")]
    [ApiController]
    [Authorize]
    public class CookBookCrudController : Controller
    {
        private readonly ICookBookService _cookBookService;

        public CookBookCrudController(ICookBookService cookBookService)
        {
            _cookBookService = cookBookService;
        }

        [HttpGet("get-cookbook")]
        public async Task<IActionResult> GetCookBook()
        {
            var name = User.FindFirstValue(ClaimTypes.Name);
            var cookBook = await _cookBookService.GetCookBook(name);
            return Ok(cookBook);
        }

        [HttpPost("update-cookbook")]
        public async Task<IActionResult> UpdateCookBook(UserCookBook cookBook)
        {
            var result = await _cookBookService.UpdateCookbook(cookBook);
            return result ? Ok() : BadRequest();
        }

        [HttpPost("add-to-shoppinglist")]
        public async Task<IActionResult> AddToShoppinglist([FromBody] SingleShopList shopList)
        {
            var name = User.FindFirstValue(ClaimTypes.Name);
            var cookBook = await _cookBookService.GetCookBook(name);

            //add recipes to current shoppinglist
            bool successfullyAddedRecipes = await _cookBookService.AddRecipesToShoppingList(cookBook,shopList);

            return successfullyAddedRecipes ? Ok() : BadRequest();
        }

        [HttpGet("get-shoppinglist")]
        public async Task<ShoppingListPageDTO> RecieveShoppingList()
        {
            var name = User.FindFirstValue(ClaimTypes.Name);
            var list = await _cookBookService.FetchShoppingList(name);
            var additonalItems = await _cookBookService.FetchAdditonalItemsFromCosmos(name);
            Console.WriteLine("size: " + additonalItems);
            ShoppingListPageDTO shoppingPageData = new ShoppingListPageDTO()
            {
                SingleShopList = list,
                AdditionalItems = additonalItems
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
