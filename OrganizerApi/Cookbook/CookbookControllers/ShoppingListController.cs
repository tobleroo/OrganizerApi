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

        private readonly IShoppinglistService _shoppinglistService;
        public ShoppingListController(IShoppinglistService shoppinglistService)
        {
            _shoppinglistService = shoppinglistService;
        }

        [HttpPost("create-shoppinglist")]
        public async Task<ActionResult<List<ShoppingListRecipeDetails>>> CreateShoppingList([FromBody] List<ShoppingListDetailsDTO> recipiesToUse)
        {
            try
            {
                var name = User.FindFirstValue(ClaimTypes.Name);
                var newShoppingListRecipies = await _shoppinglistService.CreateShoppingListFromRecipies(name, recipiesToUse);
                return Ok(newShoppingListRecipies);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add-to-shoppinglist")]
        public async Task<IActionResult> AddToShoppinglist([FromBody] SingleShopList shopList)
        {
            var name = User.FindFirstValue(ClaimTypes.Name);
            var successOrNot = await _shoppinglistService.AddDataToShoppingList(name, shopList);

            return successOrNot ? Ok("Groceries has been added!") : BadRequest("Error adding to shoppinglist");
        }

        [HttpGet("get-shoppinglist")]
        public async Task<IActionResult> RecieveShoppingList()
        {
            var name = User.FindFirstValue(ClaimTypes.Name);

            try
            {
                var shoppList = await _shoppinglistService.GetShoppingListData(name);
                return Ok(shoppList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("update-shoppinglist")]
        public async Task<IActionResult> UpdateShopingListItems(ShoppingListPageDTO updatedList)
        {
            var name = User.FindFirstValue(ClaimTypes.Name);
            var success = await _shoppinglistService.UpdateShoppingListData(name, updatedList);
            return Ok(success);
        }
    }
}
