using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizerApi.Cookbook.CookModels;
using OrganizerApi.Cookbook.CookModels.CookbookDTOs;
using OrganizerApi.Cookbook.CookRepository;
using OrganizerApi.Cookbook.CookServices;
using OrganizerBlazor.Models;
using System.Security.Claims;

namespace OrganizerApi.Cookbook.CookCrudControllers
{
    [ApiController]
    [Route("meal")]
    [Authorize]
    public class MealPlanerController : Controller
    {

        //get cookbook repository
        private readonly ICookBookRepository _cookbookRepository;
        public MealPlanerController(ICookBookRepository cookBookRepository)
        {
            _cookbookRepository = cookBookRepository;
        }

        [HttpPost("easy")]
        public async Task<ActionResult<List<Recipe>>> CreateEasyMealPLan([FromBody] List<RecipeRequestEasyDTO> desiredRecipeTypes)
        {
            try
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
            catch (Exception ex)
            {
                // Handle the exception and return an appropriate response
                return BadRequest("An error occurred: " + ex.Message);
            }
        }

        [HttpPost("shoppinglist")]
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

        [HttpGet("specific-Meal-Gen-Recipe-Details")]
        public async Task<ActionResult<Dictionary<string, List<SpecificRecipeForMealGenDetails>>>> GetRecipeDetailsForSpecificGen()
        {
            var name = User.FindFirstValue(ClaimTypes.Name);
            var cookBook = await _cookbookRepository.GetCookBook(name);
            if (cookBook == null)
            {
                return NotFound();
            }

            var recipeNamesList = MealPlanner.CreateRecipeListForSepcificGenerator(cookBook);
            return Ok(recipeNamesList);
        }
    }
}
