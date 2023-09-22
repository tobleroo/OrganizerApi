using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizerApi.Cookbook.CookModels;
using OrganizerApi.Cookbook.CookRepository;
using OrganizerApi.Cookbook.CookServices;
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

        [HttpGet("/random-list")]
        public async Task<ActionResult<List<Recipe>>> GetRecipesForWeek()
        {
            var name = User.FindFirstValue(ClaimTypes.Name);
            var cookBook = await _cookbookRepository.GetCookBook(name);
            if (cookBook == null)
            {
                return NotFound();
            }

            var listOfRecipes = MealPlanner.GetRandomMeals(cookBook);
            return listOfRecipes == null ? NotFound() : Ok(listOfRecipes);
        }
    }
}
