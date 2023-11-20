using Microsoft.Azure.Cosmos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OrganizerApi.Cookbook.CookModels;
using OrganizerApi.Cookbook.CookModels.CookbookDTOs;
using OrganizerApi.Cookbook.CookModels.CookbookDTOs.shoppinglist;
using OrganizerApi.Cookbook.CookRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OrganizerApi.Cookbook.CookRepository.Tests
{
    [TestClass()]
    public class CookBookRepositoryTests
    {

        //Mock mockContainer = new Mock<Container>();

        [TestMethod()]
        public void CookBookRepositoryTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public async Task GetCookBookTest()
        {
            // Arrange
            var mockContainer = new Mock<Container>();
            var expectedCookBook = new UserCookBook() { OwnerUsername = "Test" };

            // Create a mock FeedIterator
            var mockFeedIterator = new Mock<FeedIterator<UserCookBook>>();
            mockContainer.Setup(c => c.GetItemQueryIterator<UserCookBook>(It.IsAny<QueryDefinition>(), null, null))
                         .Returns(mockFeedIterator.Object);

            // Setup mockFeedIterator to simulate database call
            mockFeedIterator.SetupSequence(_ => _.HasMoreResults)
                            .Returns(true)  // true for the first call
                            .Returns(false); // false for subsequent calls

            var mockFeedResponse = Mock.Of<FeedResponse<UserCookBook>>(_ =>
                _.GetEnumerator() == ((IEnumerable<UserCookBook>)new[] { expectedCookBook }).GetEnumerator());

            mockFeedIterator.Setup(_ => _.ReadNextAsync(It.IsAny<CancellationToken>()))
                            .ReturnsAsync(mockFeedResponse);

            var cookBookRepository = new CookBookRepository(mockContainer.Object);

            // Act
            var result = await cookBookRepository.GetCookBook("Test");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Test", result.OwnerUsername);
        }

        [TestMethod()]
        public async Task UpdateCookBookTest()
        {
            // Arrange
            var mockContainer = new Mock<Container>();
            var testCookBook = new UserCookBook() { OwnerUsername = "test1"};

            var mockItemResponse = new Mock<ItemResponse<UserCookBook>>();
            mockItemResponse.SetupGet(r => r.StatusCode).Returns(HttpStatusCode.OK);
            mockContainer.Setup(c => c.UpsertItemAsync(
                    It.IsAny<UserCookBook>(),
                    It.IsAny<PartitionKey>(),
                    It.IsAny<ItemRequestOptions>(),
                    It.IsAny<CancellationToken>())
                )
                .ReturnsAsync(mockItemResponse.Object);

            var cookBookRepository = new CookBookRepository(mockContainer.Object);

            // Act
            var result = await cookBookRepository.UpdateCookBook(testCookBook);

            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public async Task GetShoppingListTest()
        {
            // Arrange
            var mockContainer = new Mock<Container>();
            var expectedShoppingList = new SingleShopList() { /* Initialize properties */ };

            // Mock FeedIterator and FeedResponse
            var mockFeedIterator = new Mock<FeedIterator<SingleShopList>>();
            var mockFeedResponse = Mock.Of<FeedResponse<SingleShopList>>(_ =>
                _.GetEnumerator() == ((IEnumerable<SingleShopList>)new[] { expectedShoppingList }).GetEnumerator());

            mockContainer.Setup(c => c.GetItemQueryIterator<SingleShopList>(It.IsAny<QueryDefinition>(), null, null))
                         .Returns(mockFeedIterator.Object);
            mockFeedIterator.Setup(_ => _.ReadNextAsync(It.IsAny<CancellationToken>()))
                            .ReturnsAsync(mockFeedResponse);
            mockFeedIterator.SetupSequence(_ => _.HasMoreResults).Returns(true).Returns(false);

            var cookBookRepository = new CookBookRepository(mockContainer.Object);

            // Act
            var result = await cookBookRepository.GetShoppingList("testUser", "testCookbookId");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedShoppingList, result);
        }

        [TestMethod()]
        public async Task FetchUserCookbookIdTest()
        {
            // Arrange
            var mockContainer = new Mock<Container>();
            string expectedId = "testId";

            // Mock FeedIterator and FeedResponse
            var mockFeedIterator = new Mock<FeedIterator<string>>();
            var mockFeedResponse = Mock.Of<FeedResponse<string>>(_ =>
                _.GetEnumerator() == ((IEnumerable<string>)new[] { expectedId }).GetEnumerator());

            mockContainer.Setup(c => c.GetItemQueryIterator<string>(It.IsAny<QueryDefinition>(), null, null))
                         .Returns(mockFeedIterator.Object);
            mockFeedIterator.Setup(_ => _.ReadNextAsync(It.IsAny<CancellationToken>()))
                            .ReturnsAsync(mockFeedResponse);
            mockFeedIterator.SetupSequence(_ => _.HasMoreResults).Returns(true).Returns(false);

            var cookBookRepository = new CookBookRepository(mockContainer.Object);

            // Act
            var result = await cookBookRepository.FetchUserCookbookId("testUser");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedId, result);
        }

        [TestMethod()]
        public async Task FetchRecipiesOverviewTest()
        {
            // Arrange
            var mockContainer = new Mock<Container>();
            var expectedRecipes = new List<RecipeOverviewData>() {
                new RecipeOverviewData{
                    Id = "id1",
                    Name = "testObj1",
                    TimeToCook = 10
                },new RecipeOverviewData{
                    Id = "id2",
                    Name = "testObj2",
                    TimeToCook = 20
                }
            };

            var mockFeedIterator = new Mock<FeedIterator<RecipeOverviewData>>();
            mockContainer.Setup(c => c.GetItemQueryIterator<RecipeOverviewData>(It.IsAny<QueryDefinition>(), null, null))
                         .Returns(mockFeedIterator.Object);

            var mockFeedResponse = new Mock<FeedResponse<RecipeOverviewData>>();
            mockFeedResponse.Setup(_ => _.GetEnumerator()).Returns(expectedRecipes.GetEnumerator());

            mockFeedIterator.Setup(_ => _.ReadNextAsync(It.IsAny<CancellationToken>()))
                            .ReturnsAsync(mockFeedResponse.Object);
            mockFeedIterator.SetupSequence(_ => _.HasMoreResults).Returns(true).Returns(false);

            var cookBookRepository = new CookBookRepository(mockContainer.Object);

            // Act
            var result = await cookBookRepository.FetchRecipiesOverview("testUser");

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(2 == result.Count);
            Assert.IsTrue(result.SequenceEqual(expectedRecipes));
        }

        [TestMethod()]
        public async Task FetchOneRecipeTest()
        {
            var mockContainer = new Mock<Container>();
            var expectedRecipe = new Recipe { 
                RecipeName = "test",
            };
            string testUsername = "test";
            string testRecipeId = expectedRecipe.Guid.ToString();

            var mockFeedIterator = new Mock<FeedIterator<Recipe>>();
            mockContainer.Setup(c => c.GetItemQueryIterator<Recipe>(
                    It.IsAny<QueryDefinition>(),
                    It.IsAny<string>(),
                    It.IsAny<QueryRequestOptions>())
                )
                .Returns(mockFeedIterator.Object);

            var mockFeedResponse = new Mock<FeedResponse<Recipe>>();
            mockFeedResponse.Setup(_ => _.GetEnumerator()).Returns(new List<Recipe> { expectedRecipe }.GetEnumerator());

            mockFeedIterator.Setup(_ => _.ReadNextAsync(It.IsAny<CancellationToken>()))
                            .ReturnsAsync(mockFeedResponse.Object);
            mockFeedIterator.SetupSequence(_ => _.HasMoreResults).Returns(true).Returns(false);

            var cookBookRepository = new CookBookRepository(mockContainer.Object);

            // Act
            var result = await cookBookRepository.FetchOneRecipe(testUsername, testRecipeId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedRecipe, result);
        }
    }
}