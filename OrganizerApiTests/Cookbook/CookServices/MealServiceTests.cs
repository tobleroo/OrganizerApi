using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OrganizerApi.Cookbook.CookModels;
using OrganizerApi.Cookbook.CookModels.CookbookDTOs;
using OrganizerApi.Cookbook.CookRepository;
using OrganizerApi.Cookbook.CookServices;
using OrganizerBlazor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizerApi.Cookbook.CookServices.Tests
{
    [TestClass()]
    public class MealServiceTests
    {
        

        [TestMethod()]
        public async Task GenerateEasyMealPlanTest()
        {
            // Arrange
            var mockRepository = new Mock<ICookBookRepository>();
            var testUsername = "testUser";
            var testCookBook = new UserCookBook { OwnerUsername = "testUser",
                Recipes = new List<Recipe>()
                {
                    new Recipe(){ RecipeName = "recipe1"},
                    new Recipe(){ RecipeName = "recipe2"},
                    new Recipe(){ RecipeName = "recipe3"}
                }
            };

            mockRepository.Setup(repo => repo.GetCookBook(testUsername))
                          .ReturnsAsync(testCookBook);

            var mealService = new MealService(mockRepository.Object);
            var desiredRecipeTypes = new List<RecipeRequestEasyDTO> { 
                new RecipeRequestEasyDTO (){ Difficulty = "Easy", Category = "Breakfast"},
                new RecipeRequestEasyDTO (){ Difficulty = "Medium", Category = "Lunch"},
                new RecipeRequestEasyDTO (){ Difficulty = "Hard", Category = "Dinner"},
            };

            // Act
            var result = await mealService.GenerateEasyMealPlan(testUsername, desiredRecipeTypes);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any()); // Assuming the result should have recipes
        }

        [TestMethod()]
        public async Task GetRecipeNamesForSpecificGeneratorTest()
        {
            // Arrange
            var mockRepository = new Mock<ICookBookRepository>();
            var testUsername = "testUser";
            var testCookBook = new UserCookBook
            {
                OwnerUsername = "testUser",
                Recipes = new List<Recipe>()
                {
                    new Recipe(){ RecipeName = "recipe1"},
                    new Recipe(){ RecipeName = "recipe2"},
                    new Recipe(){ RecipeName = "recipe3"}
                }
            };

            mockRepository.Setup(repo => repo.GetCookBook(testUsername))
                          .ReturnsAsync(testCookBook);

            var mealService = new MealService(mockRepository.Object);

            // Act
            var result = await mealService.GetRecipeNamesForSpecificGenerator(testUsername);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any()); // Verify that the dictionary contains data
        }

        [TestMethod()]
        public async Task GenerateSpecficMealPlanTest()
        {
            // Arrange
            var mockRepository = new Mock<ICookBookRepository>();
            var testUsername = "testUser";
            var testCookBook = new UserCookBook
            {
                OwnerUsername = "testUser",
                Recipes = new List<Recipe>()
                {
                    new Recipe(){ RecipeName = "recipe1"},
                    new Recipe(){ RecipeName = "recipe2"},
                    new Recipe(){ RecipeName = "recipe3"}
                }
            };

            mockRepository.Setup(repo => repo.GetCookBook(testUsername))
                          .ReturnsAsync(testCookBook);

            var mealService = new MealService(mockRepository.Object);
            var specificRecipesWanted = new List<RecipeRequestSpecificDTO> {
               new RecipeRequestSpecificDTO(){ Name = "recipe2"}
            };

            // Act
            var result = await mealService.GenerateSpecficMealPlan(testUsername, specificRecipesWanted);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any()); // Assuming the result should have recipes
            Assert.IsTrue(result[0].RecipeName == "recipe2");
        }
    }
}