using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizerApi.Cookbook.CookModels;
using OrganizerApi.Cookbook.CookModels.CookbookDTOs;
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

        [HttpPost("update-shoppinglist")]
        public async Task<IActionResult> UpdateShoppinglist([FromBody] ShoppingList shoppingList)
        {
            var name = User.FindFirstValue(ClaimTypes.Name);
            var cookBook = await _cookBookService.GetCookBook(name);

            //add the additional items to public list with all items
            await _cookBookService.AddNewAdditonalItemsToCookbook(name, cookBook.PreviouslyAddedAdditonalItems, shoppingList.AdditionalItems);

            var res = await _cookBookService.UpdateShopppingListOfCookbook(cookBook, shoppingList);
            return res ? Ok() : BadRequest();
        }

        [HttpGet("get-shoppinglist")]
        public async Task<ShoppingListALLItems> RecieveShoppingList()
        {
            var name = User.FindFirstValue(ClaimTypes.Name);
            var list = await _cookBookService.FetchShoppingList(name);
            return list;
        }
    }
}
