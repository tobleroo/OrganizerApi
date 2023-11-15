using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Linq;
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
            try
            {
                var cookBook = await _cookBookService.GetCookBook(name);
                return Ok(cookBook);
            }catch (Exception ex)
            {
                return BadRequest("something went wrong -> " + ex.Message);
            }
        }

        [HttpGet("recipe-overview-data")]
        public async Task<IActionResult> GetRecipiesOverviewData()
        {
            var name = User.FindFirstValue(ClaimTypes.Name);

            try
            {
                var recipesOverviewData = await _cookBookService.FetchRecipeOverviewData(name);
                return Ok(recipesOverviewData);
            }
            catch (Exception ex)
            {
                return BadRequest("something went wrong -> " + ex.Message);
            }
        }

        [HttpPost("update-cookbook")]
        public async Task<IActionResult> UpdateCookBook(UserCookBook cookBook)
        {
            try
            {
                // Returns a bool to know if it was a successful update
                var result = await _cookBookService.UpdateCookbook(cookBook);
                return result ? Ok() : BadRequest();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return BadRequest("Something went wrong -> " + ex.Message);
            }
        }

        [HttpGet("get-one-recipe/{recipeId}")]
        public async Task<IActionResult> GetSingleRecipe(string recipeId)
        {
            var name = User.FindFirstValue(ClaimTypes.Name);
            try
            {
                Recipe recipeWanted = await _cookBookService.GetOneRecipe(name, recipeId);
                return recipeWanted.IsNull() ? Ok(recipeWanted) : NotFound();

            }catch (Exception ex) { return BadRequest("something went wrong -> " + ex.Message); }

        }

        [HttpPost("add-one-recipe")]
        public async Task<IActionResult> AddOneRecipeToCookbook([FromBody]Recipe recipe)
        {

            try
            {
                var name = User.FindFirstValue(ClaimTypes.Name);

                var success = await _cookBookService.AddOneRecipeToCookbook(name, recipe);
                return success ? Ok() : BadRequest();

            } catch (Exception ex) { return BadRequest("something went wrong -> " + ex.Message); }
        }

        [HttpPost("remove-one-recipe")]
        public async Task<IActionResult> RemoveOneRecipe([FromBody] string recipeId)
        {
            try
            {
                var name = User.FindFirstValue(ClaimTypes.Name);
                var success = await _cookBookService.RemoveOneRecipeFromCookbook(recipeId, name);
                return success ? Ok() : NotFound();

            } catch (Exception ex) { return BadRequest("something went wrong -> " + ex.Message); }
        }
    }
}
