using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OrganizerApi.Cookbook.CookModels.CookbookDTOs.shoppinglist;
using OrganizerApi.Cookbook.CookModels;
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
    public class ShoppinglistServiceTests
    {

        [TestMethod()]
        public async Task CreateShoppingListFromRecipiesTest()
        {
            var mockRepository = new Mock<ICookBookRepository>();
            var testUsername = "testUser";
            var testCookBook = new UserCookBook
            {
                OwnerUsername = "testUser",
                Recipes = new List<Recipe>()
                {
                    new Recipe()
                    {
                        RecipeName = "recipe1",
                        Ingredients = new List<Ingredient>
                        {
                            new Ingredient { Name = "Flour", Quantity = 2, Unit = "cups" },
                            new Ingredient { Name = "Sugar", Quantity = 2, Unit = "cup" },
                            new Ingredient { Name = "Eggs", Quantity = 2, Unit = "pieces" }
                        }
                    },
                    new Recipe()
                    {
                        RecipeName = "recipe2",
                        Ingredients = new List<Ingredient>
                        {
                            new Ingredient { Name = "Tomatoes", Quantity = 3, Unit = "pieces" },
                            new Ingredient { Name = "Olive Oil", Quantity = 3, Unit = "tablespoons" },
                            new Ingredient { Name = "Basil", Quantity = 3, Unit = "leaves" }
                        }
                    }
                }
            };

            mockRepository.Setup(repo => repo.GetCookBook(testUsername))
                          .ReturnsAsync(testCookBook);

            var shoppinglistService = new ShoppinglistService(mockRepository.Object);
            var recipiesToUse = new List<ShoppingListDetailsDTO> { 
                new ShoppingListDetailsDTO(){ RecipeName = "recipe1", PortionsAmount = 2},
                new ShoppingListDetailsDTO(){ RecipeName = "recipe2", PortionsAmount = 3}
            };

            // Act
            var result = await shoppinglistService.CreateShoppingListFromRecipies(testUsername, recipiesToUse);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            Assert.AreEqual(4, result[0].Ingredients[0].Quantity);
            Assert.AreEqual(9, result[1].Ingredients[1].Quantity);
        }

        [TestMethod()]
        public async Task AddDataToShoppingListTest()
        {
            // Arrange
            var mockRepository = new Mock<ICookBookRepository>();
            var testUsername = "testUser";
            var testCookBook = new UserCookBook
            {
                OwnerUsername = "testUser",
                Recipes = new List<Recipe>()
                {
                    new Recipe()
                    {
                        RecipeName = "recipe1",
                        Ingredients = new List<Ingredient>
                        {
                            new Ingredient { Name = "Flour", Quantity = 2, Unit = "cups" },
                            new Ingredient { Name = "Sugar", Quantity = 2, Unit = "cup" },
                            new Ingredient { Name = "Eggs", Quantity = 2, Unit = "pieces" }
                        }
                    },
                    new Recipe()
                    {
                        RecipeName = "recipe2",
                        Ingredients = new List<Ingredient>
                        {
                            new Ingredient { Name = "Tomatoes", Quantity = 3, Unit = "pieces" },
                            new Ingredient { Name = "Olive Oil", Quantity = 3, Unit = "tablespoons" },
                            new Ingredient { Name = "Basil", Quantity = 3, Unit = "leaves" }
                        }
                    }
                }
            };

            mockRepository.Setup(repo => repo.GetCookBook(testUsername))
                          .ReturnsAsync(testCookBook);
            mockRepository.Setup(repo => repo.UpdateCookBook(It.IsAny<UserCookBook>()))
                          .ReturnsAsync(true);

            var shoppinglistService = new ShoppinglistService(mockRepository.Object);
            var shopList = new SingleShopList {
                SingleShopListRecipes = new List<SingleShopListRecipe>() 
                {
                    new SingleShopListRecipe {RecipeName = "recipeWanted1"}
                } 
            };

            // Act
            await shoppinglistService.AddDataToShoppingList(testUsername, shopList);

            var result = await shoppinglistService.GetShoppingListData(testUsername);



            // Assert
            Assert.IsTrue(result.SingleShopList.SingleShopListRecipes[0].RecipeName == "recipeWanted1");
            mockRepository.Verify(repo => repo.UpdateCookBook(It.IsAny<UserCookBook>()), Times.Once);
        }

        [TestMethod()]
        public async Task GetShoppingListDataTest()
        {
            // Arrange
            var mockRepository = new Mock<ICookBookRepository>();
            var testUsername = "testUser";
            var testCookBook = new UserCookBook
            {
                OwnerUsername = "testUser",
                Recipes = new List<Recipe>()
                {
                    new Recipe()
                    {
                        RecipeName = "recipe1",
                        Ingredients = new List<Ingredient>
                        {
                            new Ingredient { Name = "Flour", Quantity = 2, Unit = "cups" },
                            new Ingredient { Name = "Sugar", Quantity = 2, Unit = "cup" },
                            new Ingredient { Name = "Eggs", Quantity = 2, Unit = "pieces" }
                        }
                    },
                    new Recipe()
                    {
                        RecipeName = "recipe2",
                        Ingredients = new List<Ingredient>
                        {
                            new Ingredient { Name = "Tomatoes", Quantity = 3, Unit = "pieces" },
                            new Ingredient { Name = "Olive Oil", Quantity = 3, Unit = "tablespoons" },
                            new Ingredient { Name = "Basil", Quantity = 3, Unit = "leaves" }
                        }
                    }
                },
                ShoppingList = new SingleShopList()
                {
                    ListName = "Shopping list",
                    SingleShopListRecipes = new List<SingleShopListRecipe> { new SingleShopListRecipe { RecipeName = "recipe1"},
                    new SingleShopListRecipe { RecipeName = "shopRecipe1"}}
                }
            };

            mockRepository.Setup(repo => repo.GetCookBook(testUsername))
                          .ReturnsAsync(testCookBook);

            var shoppinglistService = new ShoppinglistService(mockRepository.Object);

            // Act
            var result = await shoppinglistService.GetShoppingListData(testUsername);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Shopping list",result.SingleShopList.ListName);
        }

        [TestMethod()]
        public async Task UpdateShoppingListDataTest()
        {
            // Arrange
            var mockRepository = new Mock<ICookBookRepository>();
            var testUsername = "testUser";
            var testCookBook = new UserCookBook
            {
                OwnerUsername = "testUser",
                Recipes = new List<Recipe>()
                {
                    new Recipe()
                    {
                        RecipeName = "recipe1",
                        Ingredients = new List<Ingredient>
                        {
                            new Ingredient { Name = "Flour", Quantity = 2, Unit = "cups" },
                            new Ingredient { Name = "Sugar", Quantity = 2, Unit = "cup" },
                            new Ingredient { Name = "Eggs", Quantity = 2, Unit = "pieces" }
                        }
                    },
                    new Recipe()
                    {
                        RecipeName = "recipe2",
                        Ingredients = new List<Ingredient>
                        {
                            new Ingredient { Name = "Tomatoes", Quantity = 3, Unit = "pieces" },
                            new Ingredient { Name = "Olive Oil", Quantity = 3, Unit = "tablespoons" },
                            new Ingredient { Name = "Basil", Quantity = 3, Unit = "leaves" }
                        }
                    }
                }
            };

            mockRepository.Setup(repo => repo.GetCookBook(testUsername))
                          .ReturnsAsync(testCookBook);
            mockRepository.Setup(repo => repo.UpdateCookBook(It.IsAny<UserCookBook>()))
                          .ReturnsAsync(true);

            var shoppinglistService = new ShoppinglistService(mockRepository.Object);
            var recipeToAdd1 = new SingleShopListRecipe
            {
                RecipeName = "recipeToAdd1"
            };

            // Add ingredients to the recipeToAdd1
            recipeToAdd1.Ingredients.Add(new Ingredient { Name = "Ingredient1", Quantity = 2.5, Unit = "Cups" });
            recipeToAdd1.Ingredients.Add(new Ingredient { Name = "Ingredient2", Quantity = 1, Unit = "Tablespoon" });

            // Create another SingleShopListRecipe
            var recipeToAdd2 = new SingleShopListRecipe
            {
                RecipeName = "recipeToAdd2"
            };

            // Add ingredients to the recipeToAdd2
            recipeToAdd2.Ingredients.Add(new Ingredient { Name = "Ingredient3", Quantity = 3, Unit = "Pieces" });
            recipeToAdd2.Ingredients.Add(new Ingredient { Name = "Ingredient4", Quantity = 0.5, Unit = "Pound" });

            // Create the newShoppingList with the added recipes and ingredients
            var newShoppingList = new ShoppingListPageDTO
            {
                SingleShopList = new SingleShopList
                {
                SingleShopListRecipes = new List<SingleShopListRecipe>
                    {
                        recipeToAdd1,
                        recipeToAdd2
                    }
                }
            };

            // Act
            await shoppinglistService.UpdateShoppingListData(testUsername, newShoppingList);
            var result = await shoppinglistService.GetShoppingListData(testUsername);

            // Assert

            Assert.AreEqual("recipeToAdd1", result.SingleShopList.SingleShopListRecipes[0].RecipeName);
            mockRepository.Verify(repo => repo.UpdateCookBook(It.Is<UserCookBook>(cb => cb.ShoppingList == newShoppingList.SingleShopList)), Times.Once);
        }
    }
}