using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizerApi.Cookbook.CookModels;
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

        [HttpGet("recipe-overview-data")]
        public async Task<IActionResult> GetRecipiesOverviewData()
        {
            var name = User.FindFirstValue(ClaimTypes.Name);

            var recipesOverviewData = await _cookBookService.FetchRecipeOverviewData(name);
            return Ok(recipesOverviewData);
        }

        [HttpPost("update-cookbook")]
        public async Task<IActionResult> UpdateCookBook(UserCookBook cookBook)
        {
            var result = await _cookBookService.UpdateCookbook(cookBook);
            return result ? Ok() : BadRequest();
        }

        [HttpGet("get-one-recipe/{recipeId}")]
        public async Task<IActionResult> GetSingleRecipe(string recipeId)
        {
            var name = User.FindFirstValue(ClaimTypes.Name);

            Recipe recipeWanted = await _cookBookService.GetOneRecipe(name, recipeId);
            return Ok(recipeWanted);
        }




    }
}
