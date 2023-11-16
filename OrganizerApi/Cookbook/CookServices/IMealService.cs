using Microsoft.AspNetCore.Mvc;
using OrganizerApi.Cookbook.CookModels;
using OrganizerApi.Cookbook.CookModels.CookbookDTOs;
using OrganizerBlazor.Models;

namespace OrganizerApi.Cookbook.CookServices
{
    public interface IMealService
    {
        Task<List<Recipe>?> GenerateEasyMealPlan(string username, List<RecipeRequestEasyDTO> desiredRecipeTypes);
        Task<List<Recipe>> GenerateSpecficMealPlan(string username, List<RecipeRequestSpecificDTO> specificRecipesWanted);
        Task<Dictionary<string, List<SpecificRecipeForMealGenDetails>>> GetRecipeNamesForSpecificGenerator(string username);
    }
}
