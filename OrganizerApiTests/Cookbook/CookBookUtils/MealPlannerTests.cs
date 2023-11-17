using Microsoft.VisualStudio.TestPlatform.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrganizerApi.Cookbook.CookBookUtils;
using OrganizerApi.Cookbook.CookModels;
using OrganizerApi.Cookbook.CookModels.CookbookDTOs;
using OrganizerBlazor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizerApi.Cookbook.CookBookUtils.Tests
{
    [TestClass()]
    public class MealPlannerTests
    {
        [TestMethod()]
        public void CreateEasyMealPlan_Working()
        {
            // wassap mafakka

            List<RecipeRequestEasyDTO> requestData = new()
            {
                new RecipeRequestEasyDTO(){
                    Difficulty = "Easy",
                    Category = "Breakfast",
                    MaxCookTime = 100
                },
                new RecipeRequestEasyDTO(){
                    Difficulty = "Medium",
                    Category = "Lunch",
                    MaxCookTime = 100
                },
                new RecipeRequestEasyDTO(){
                    Difficulty = "Hard",
                    Category = "Dinner",
                    MaxCookTime = 100
                }
            };

            List<Recipe> existingRecipies = new()
            {
                new Recipe(){
                    RecipeName = "recipe1",
                    Difficulty = RecipeDifficulties.Easy,
                    RecipeType = RecipeTypes.Breakfast,
                    CookTime = 1
                },
                new Recipe(){
                    RecipeName = "recipe2",
                    Difficulty = RecipeDifficulties.Medium,
                    RecipeType = RecipeTypes.Lunch,
                    CookTime = 1
                },
                new Recipe(){
                    RecipeName = "recipe3",
                    Difficulty = RecipeDifficulties.Hard,
                    RecipeType = RecipeTypes.Dinner,
                    CookTime = 1
                }
            };


            var result = MealPlanner.CreateEasyMealPlan(requestData, existingRecipies);

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("recipe1", result[0].RecipeName);
            CollectionAssert.AreEqual(existingRecipies, result);
        }

        [TestMethod]
        public void CreateEasyMealPlan_WithNullValues()
        {
            var result = MealPlanner.CreateEasyMealPlan(null, null);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void CreateEasyMealPlan_WithEmptyLists()
        {
            List<RecipeRequestEasyDTO> requestData = new();
            List<Recipe> existingRecipies = new();

            var result = MealPlanner.CreateEasyMealPlan(requestData, existingRecipies);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreateEasyMealPlan_TestEachDifficultyLevel()
        {
            List<RecipeRequestEasyDTO> requestData = new()
            {
                new RecipeRequestEasyDTO(){
                    Difficulty = "Easy",
                    Category = "Breakfast",
                    MaxCookTime = 100
                },
                new RecipeRequestEasyDTO(){
                    Difficulty = "Medium",
                    Category = "Lunch",
                    MaxCookTime = 100
                },
                new RecipeRequestEasyDTO(){
                    Difficulty = "Hard",
                    Category = "Dinner",
                    MaxCookTime = 100
                }
            };

            List<Recipe> existingRecipies = new()
            {
                new Recipe(){
                    RecipeName = "recipe1",
                    Difficulty = RecipeDifficulties.Easy,
                    RecipeType = RecipeTypes.Breakfast,
                    CookTime = 1
                },
                new Recipe(){
                    RecipeName = "recipe2",
                    Difficulty = RecipeDifficulties.Medium,
                    RecipeType = RecipeTypes.Lunch,
                    CookTime = 1
                },
                new Recipe(){
                    RecipeName = "recipe3",
                    Difficulty = RecipeDifficulties.Hard,
                    RecipeType = RecipeTypes.Dinner,
                    CookTime = 1
                }
            };

            var result = MealPlanner.CreateEasyMealPlan(requestData, existingRecipies);

            Assert.IsTrue(result.Any(r => r.Difficulty == RecipeDifficulties.Easy));
            Assert.IsTrue(result.Any(r => r.Difficulty == RecipeDifficulties.Medium));
            Assert.IsTrue(result.Any(r => r.Difficulty == RecipeDifficulties.Hard));
        }

        [TestMethod]
        public void CreateEasyMealPlan_TestEachRecipeType()
        {
            List<RecipeRequestEasyDTO> requestData = new()
            {
                new RecipeRequestEasyDTO(){
                    Difficulty = "Easy",
                    Category = "Breakfast",
                    MaxCookTime = 100
                },
                new RecipeRequestEasyDTO(){
                    Difficulty = "Medium",
                    Category = "Lunch",
                    MaxCookTime = 100
                },
                new RecipeRequestEasyDTO(){
                    Difficulty = "Hard",
                    Category = "Dinner",
                    MaxCookTime = 100
                }
            };

            List<Recipe> existingRecipies = new()
            {
                new Recipe(){
                    RecipeName = "recipe1",
                    Difficulty = RecipeDifficulties.Easy,
                    RecipeType = RecipeTypes.Breakfast,
                    CookTime = 1
                },
                new Recipe(){
                    RecipeName = "recipe2",
                    Difficulty = RecipeDifficulties.Medium,
                    RecipeType = RecipeTypes.Lunch,
                    CookTime = 1
                },
                new Recipe(){
                    RecipeName = "recipe3",
                    Difficulty = RecipeDifficulties.Hard,
                    RecipeType = RecipeTypes.Dinner,
                    CookTime = 1
                }
            };

            var result = MealPlanner.CreateEasyMealPlan(requestData, existingRecipies);

            Assert.IsTrue(result.Any(r => r.RecipeType == RecipeTypes.Breakfast));
            Assert.IsTrue(result.Any(r => r.RecipeType == RecipeTypes.Lunch));
            Assert.IsTrue(result.Any(r => r.RecipeType == RecipeTypes.Dinner));
        }

        // next method in class

        [TestMethod]
        public void CreateRecipeListForSpecificGenerator_Working()
        {

            var mockCookBook = new UserCookBook()
            {
                OwnerUsername = "admin",
                Recipes = new List<Recipe>
                {
                    new Recipe { RecipeName = "Pancakes", RecipeType = RecipeTypes.Breakfast, Guid = Guid.NewGuid() },
                    new Recipe { RecipeName = "Sandwich", RecipeType = RecipeTypes.Lunch, Guid = Guid.NewGuid() },
                    new Recipe { RecipeName = "Steak", RecipeType = RecipeTypes.Dinner, Guid = Guid.NewGuid() },
                    new Recipe { RecipeName = "Cake", RecipeType = RecipeTypes.Dessert, Guid = Guid.NewGuid() },
                    new Recipe { RecipeName = "Chips", RecipeType = RecipeTypes.Snack, Guid = Guid.NewGuid() }
                }
            };

            // Act
            var result = MealPlanner.CreateRecipeListForSepcificGenerator(mockCookBook);

            // Assert
            Assert.AreEqual(1, result["Breakfast"].Count);
            Assert.AreEqual("Pancakes", result["Breakfast"].First().RecipeName);
            Assert.AreEqual(1, result["Lunch"].Count);
            Assert.AreEqual("Sandwich", result["Breakfast"].First().RecipeName);
            Assert.AreEqual(1, result["Dinner"].Count);
            Assert.AreEqual("Steak", result["Breakfast"].First().RecipeName);
            Assert.AreEqual(1, result["Dessert"].Count);
            Assert.AreEqual("Cake", result["Breakfast"].First().RecipeName);
            Assert.AreEqual(1, result["Snack"].Count);
            Assert.AreEqual("Chips", result["Breakfast"].First().RecipeName);
        }

        [TestMethod]
        public void CreateRecipeListForSepcificGenerator_WithEmptyCookbook_ReturnsEmptyCategories()
        {
            // Arrange
            var emptyCookbook = new UserCookBook()
            {
                OwnerUsername = "admin",
                Recipes = new List<Recipe>()
            };

            // Act
            var result = MealPlanner.CreateRecipeListForSepcificGenerator(emptyCookbook);

            // Assert
            Assert.AreEqual(0, result["Breakfast"].Count);
        }

        [TestMethod]
        public void CreateRecipeListForSepcificGenerator_OneCategoryListEmpty()
        {
            var mockCookBook = new UserCookBook()
            {
                OwnerUsername = "admin",
                Recipes = new List<Recipe>
                {
                    new Recipe { RecipeName = "Pancakes", RecipeType = RecipeTypes.Breakfast, Guid = Guid.NewGuid() },
                    new Recipe { RecipeName = "Sandwich", RecipeType = RecipeTypes.Lunch, Guid = Guid.NewGuid() },
                    new Recipe { RecipeName = "Steak", RecipeType = RecipeTypes.Dinner, Guid = Guid.NewGuid() },
                    new Recipe { RecipeName = "Chips", RecipeType = RecipeTypes.Snack, Guid = Guid.NewGuid() }
                }
            };

            var result = MealPlanner.CreateRecipeListForSepcificGenerator(mockCookBook);

            Assert.IsNotNull(result["Dessert"]);
            Assert.AreEqual(0, result["Dessert"].Count);
        }

        [TestMethod]
        public void CreateSpecificMealPlan_Working()
        {
            var mockCookBook = new UserCookBook()
            {
                OwnerUsername = "admin",
                Recipes = new List<Recipe>
                {
                    new Recipe { RecipeName = "Pancakes", RecipeType = RecipeTypes.Breakfast, Guid = Guid.NewGuid() },
                    new Recipe { RecipeName = "Sandwich", RecipeType = RecipeTypes.Lunch, Guid = Guid.NewGuid() },
                    new Recipe { RecipeName = "Steak", RecipeType = RecipeTypes.Dinner, Guid = Guid.NewGuid() },
                    new Recipe { RecipeName = "Cake", RecipeType = RecipeTypes.Dessert, Guid = Guid.NewGuid() },
                    new Recipe { RecipeName = "Chips", RecipeType = RecipeTypes.Snack, Guid = Guid.NewGuid() }
                }
            };

            List<RecipeRequestSpecificDTO> wantedRecipies = new()
            {
                new RecipeRequestSpecificDTO
                {
                    Portions = 1,
                    Name = "Pancakes",
                },
                new RecipeRequestSpecificDTO
                {
                    Portions = 2,
                    Name = "Sandwich"
                }
            };

            var result = MealPlanner.CreateSpecificMealPlan(wantedRecipies, mockCookBook.Recipes);

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Pancakes", result[0].RecipeName);
            Assert.AreEqual("Sandwich", result[0].RecipeName);
        }

        [TestMethod]
        public void CreateSpecificMealPlan_WithNullValues()
        {
            var result = MealPlanner.CreateSpecificMealPlan(null, null);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }
    }
}