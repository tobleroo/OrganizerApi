using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizerApi.Cookbook.CookModels;
using OrganizerApi.Cookbook.CookRepository;
using OrganizerApi.Cookbook.CookServices;
using OrganizerBlazor.Models;
using System.Security.Claims;

namespace OrganizerApi.Cookbook.CookCrudControllers
{
    [Route("/mealplanner")]
    [ApiController]
    [Authorize]
    public class MealPlanerController : Controller
    {

        //get cookbook repository
        private readonly ICookBookRepository _cookbookRepository;
        public MealPlanerController(ICookBookRepository cookBookRepository)
        {
            _cookbookRepository = cookBookRepository;
        }

        [HttpPost("/easy")]
        public async Task<ActionResult<List<Recipe>>> CreateEasyMealPLan([FromBody] List<RecipeRequestEasyDTO> desiredRecipeTypes)
        {
            //get user name
            var name = User.FindFirstValue(ClaimTypes.Name);
            var cookBook = await _cookbookRepository.GetCookBook(name);
            if (cookBook == null)
            {
                return NotFound();
            }

            //send the list of required recipes and the cookbook to the method
            var mealplan = MealPlanner.CreateEasyMealPlan(desiredRecipeTypes, cookBook.Recipes);
            return Ok(mealplan);
        }
    }
}
