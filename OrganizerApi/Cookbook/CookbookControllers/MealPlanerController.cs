using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizerApi.Cookbook.CookBookUtils;
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
        private readonly IMealService _mealService;

        public MealPlanerController(IMealService mealService)
        {
            _mealService = mealService;
        }

        [HttpPost("easy")]
        public async Task<ActionResult<List<Recipe>>> CreateEasyMealPLan([FromBody] List<RecipeRequestEasyDTO> desiredRecipeTypes)
        {
            var name = User.FindFirstValue(ClaimTypes.Name);
            try
            {
                var mealplan = await _mealService.GenerateEasyMealPlan(name, desiredRecipeTypes);

                if (mealplan.Any()) return Ok(mealplan); // Return the meal plan

                else return NotFound("No recipes found for the specified criteria.");

            }catch (InvalidOperationException ex){
                return BadRequest(ex.Message);

            }catch (Exception ex){
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred."+ ex);

            }
        }

        [HttpGet("specific-Meal-Gen-Recipe-Details")]
        public async Task<ActionResult<Dictionary<string, List<SpecificRecipeForMealGenDetails>>>> GetRecipeDetailsForSpecificGen()
        {
            var name = User.FindFirstValue(ClaimTypes.Name);
            try
            {
                var listOfRecipeNamesAndCetogories = await _mealService.GetRecipeNamesForSpecificGenerator(name);
                return Ok(listOfRecipeNamesAndCetogories);
            } catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred." + ex.Message);
            }
            
        }

        [HttpPost("specific")]
        public async Task<ActionResult<List<Recipe>>> PostSpecificMealGenerator([FromBody] List<RecipeRequestSpecificDTO> specificRecipesWanted){
            try
            {
                //get user name
                var name = User.FindFirstValue(ClaimTypes.Name);

                var mealPlan = await _mealService.GenerateSpecficMealPlan(name, specificRecipesWanted);
                //send the list of required recipes and the cookbook to the method
                return Ok(mealPlan);
            }
            catch (Exception ex)
            {
                // Handle the exception and return an appropriate response
                return BadRequest("An error occurred: " + ex.Message);
            }
        }
    }
}
