using Microsoft.Azure.Cosmos.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OrganizerApi.Cookbook.CookModels;
using OrganizerApi.Cookbook.CookModels.CookbookDTOs;
using OrganizerApi.Cookbook.CookRepository;
using OrganizerApi.Cookbook.CookServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizerApi.Cookbook.CookServices.Tests
{
    [TestClass()]
    public class CookbookServiceTests
    {
        [TestMethod()]
        public void CookbookServiceTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public async Task GetCookBook_success()
        {
            // Arrange
            var mockRepository = new Mock<ICookBookRepository>();
            var expectedUserCookBook = new UserCookBook() { OwnerUsername = "test" };

            mockRepository.Setup(repo => repo.GetCookBook(It.IsAny<string>()))
                          .ReturnsAsync(expectedUserCookBook);

            var cookbookService = new CookbookService(mockRepository.Object);

            var result = await cookbookService.GetCookBook("test");

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedUserCookBook, result);
        }

        [TestMethod()]
        public async Task UpdateCookbook_Success()
        {
            // Arrange
            var mockRepository = new Mock<ICookBookRepository>();
            var oldUserCookBook = new UserCookBook() { OwnerUsername = "test" };

            mockRepository.Setup(repo => repo.GetCookBook(It.IsAny<string>()))
                          .ReturnsAsync(oldUserCookBook);

            var cookbookService = new CookbookService(mockRepository.Object);

            var oldRecipe = await cookbookService.GetCookBook("test");

            oldRecipe.OwnerUsername = "test update";

            await cookbookService.UpdateCookbook(oldRecipe);

            var result = await cookbookService.GetCookBook("test update");

            Assert.IsNotNull(result);
            Assert.AreEqual("test update", result.OwnerUsername);
        }

        [TestMethod()]
        public async Task AddOneRecipeToCookbook()
        {
            var mockRepository = new Mock<ICookBookRepository>();

            var oldUserCookBook = new UserCookBook() { OwnerUsername = "test" };
            var mockRecipeExisting = new Recipe() { RecipeName = "existing" };
            oldUserCookBook.Recipes.Add(mockRecipeExisting);

            var mockRecipeToReplace = new Recipe() { RecipeName = "test replace", Guid = mockRecipeExisting.Guid };
            var mockAddiotionalRecipeToAdd = new Recipe() { RecipeName = "recipe 2" };

            mockRepository.Setup(repo => repo.GetCookBook("test"))
                          .ReturnsAsync(oldUserCookBook);
            mockRepository.Setup(repo => repo.UpdateCookBook(It.IsAny<UserCookBook>()))
                  .ReturnsAsync(true);

            var service = new CookbookService(mockRepository.Object);

            // try replace the old with the new, ones who have same guid id
            await service.AddOneRecipeToCookbook("test", mockRecipeToReplace);
            // add a new recipe to list
            await service.AddOneRecipeToCookbook("test", mockAddiotionalRecipeToAdd);

            var result = await service.GetCookBook("test");

            Assert.AreEqual(2, result.Recipes.Count);
            Assert.AreEqual("test replace", result.Recipes[0].RecipeName);
        }

        [TestMethod()]
        public async Task FetchRecipeOverviewData()
        {
            var mockRepository = new Mock<ICookBookRepository>();
            var testUsername = "testUser";
            var expectedData = new List<RecipeOverviewData>
            {
                new RecipeOverviewData() { Name = "recipeOverview1" },
                new RecipeOverviewData() { Name = "recipeOverview2" }

            };

            mockRepository.Setup(repo => repo.FetchRecipiesOverview(testUsername))
                          .ReturnsAsync(expectedData);

            var cookbookService = new CookbookService(mockRepository.Object);

            // Act
            var result = await cookbookService.FetchRecipeOverviewData(testUsername);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedData.Count, result.Count);
            CollectionAssert.AreEqual(expectedData, result.ToList());
        }

        [TestMethod()]
        public async Task GetOneRecipe()
        {
            // Arrange
            var mockRepository = new Mock<ICookBookRepository>();
            var testUsername = "testUser";
            var testRecipeId = "testRecipeId";
            var expectedRecipe = new Recipe { RecipeName = "test recipe"};

            mockRepository.Setup(repo => repo.FetchOneRecipe(testUsername, testRecipeId))
                          .ReturnsAsync(expectedRecipe);

            var cookbookService = new CookbookService(mockRepository.Object);

            // Act
            var result = await cookbookService.GetOneRecipe(testUsername, testRecipeId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedRecipe, result);
            Assert.IsTrue("test recipe" == result.RecipeName);
        }

        [TestMethod]
        public async Task RemoveOneRecipeFromCookbook_Success()
        {
            // Arrange
            var mockRepository = new Mock<ICookBookRepository>();
            var testUsername = "testUser";
            var recipeToRemoveName = "testrecipe2";
            var testCookBook = new UserCookBook
            {
                OwnerUsername = "test",
                Recipes = new List<Recipe>
                {
                    new Recipe { RecipeName = "testrecipe1" },
                    new Recipe { RecipeName = "testrecipe2" } 
                }
            };

            mockRepository.Setup(repo => repo.GetCookBook(testUsername))
                          .ReturnsAsync(testCookBook);
            mockRepository.Setup(repo => repo.UpdateCookBook(It.IsAny<UserCookBook>()))
                          .ReturnsAsync(true);

            var cookbookService = new CookbookService(mockRepository.Object);

            // Act
            var result = await cookbookService.RemoveOneRecipeFromCookbook(recipeToRemoveName, testUsername);

            // Assert
            Assert.IsTrue(result);
            mockRepository.Verify(repo => repo.UpdateCookBook(It.Is<UserCookBook>(cb => !cb.Recipes.Any(r => r.Guid.ToString() == recipeToRemoveName))), Times.Once);
        }
    }
}