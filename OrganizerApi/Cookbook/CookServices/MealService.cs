using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using OrganizerApi.Cookbook.CookBookUtils;
using OrganizerApi.Cookbook.CookModels;
using OrganizerApi.Cookbook.CookModels.CookbookDTOs;
using OrganizerApi.Cookbook.CookRepository;
using OrganizerBlazor.Models;
using System.Security.Claims;

namespace OrganizerApi.Cookbook.CookServices
{
    public class MealService : IMealService
    {

        private readonly ICookBookRepository _cookbookRepository;


        public MealService(ICookBookRepository repository)
        {
            _cookbookRepository = repository;
        }

        public async Task<List<Recipe>?> GenerateEasyMealPlan(string username, List<RecipeRequestEasyDTO> desiredRecipeTypes)
        {
            try
            {
                var cookBook = await _cookbookRepository.GetCookBook(username);

                if (cookBook == null || cookBook.Recipes.Count == 0)
                {
                    throw new InvalidOperationException("Cookbook not found or does not contain recipes.");
                }

                var mealplan = MealPlanner.CreateEasyMealPlan(desiredRecipeTypes, cookBook.Recipes);

                if (mealplan.Any())
                {
                    return mealplan;
                }
                else
                {
                    throw new InvalidOperationException("No recipes found for the specified criteria.");
                }
            }
            catch
            {
                // Throw a more specific exception or handle it here as needed
                throw new InvalidOperationException("An error occurred while processing your request.");
            }
        }

        public async Task<Dictionary<string, List<SpecificRecipeForMealGenDetails>>> GetRecipeNamesForSpecificGenerator(string username)
        {
            try
            {
                var cookBook = await _cookbookRepository.GetCookBook(username);
                if (cookBook == null)
                {
                    throw new InvalidOperationException("Could not find cookbook");
                }


                var recipeNamesList = MealPlanner.CreateRecipeListForSepcificGenerator(cookBook);
                if(recipeNamesList.Any())
                {
                    return recipeNamesList;
                }

            }
            catch(Exception ex) 
            {
                throw new Exception("something went wrong" + ex.Message);
            }
            return new Dictionary<string, List<SpecificRecipeForMealGenDetails>>();
        }

        public async Task<List<Recipe>> GenerateSpecficMealPlan(string username, List<RecipeRequestSpecificDTO> specificRecipesWanted)
        {
            var cookBook = await _cookbookRepository.GetCookBook(username);

            return MealPlanner.CreateSpecificMealPlan(specificRecipesWanted, cookBook.Recipes);
        }
    }
}
