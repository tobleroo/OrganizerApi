using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrganizerApi.Cookbook.CookBookUtils;
using OrganizerApi.Cookbook.CookModels;
using OrganizerApi.Cookbook.CookModels.CookbookDTOs.shoppinglist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizerApi.Cookbook.CookBookUtils.Tests
{
    [TestClass()]
    public class ShoppingListCreatorTests
    {

        [TestMethod()]
        public void CreateShoppingList_ProperDataInput()
        {
            List<ShoppingListDetailsDTO> wantedRecipies = new()
            {
                new ShoppingListDetailsDTO
                {
                    RecipeName = "1",
                    PortionsAmount = 5,
                },
                new ShoppingListDetailsDTO
                {
                    RecipeName = "2",
                    PortionsAmount = 7,
                }
            };

            List<Recipe> recipiesInCookbook = new()
            {
                new Recipe
                {
                    RecipeName = "1",
                    Ingredients = new List<Ingredient>()
                    {
                        new Ingredient
                        {
                            Name = "ingrOne",
                            Quantity = 1,
                        },
                        new Ingredient
                        {
                            Name = "ingrOne",
                            Quantity = 2,
                        }
                    }
                },new Recipe
                {
                    RecipeName = "2",
                    Ingredients = new List<Ingredient>()
                    {
                        new Ingredient
                        {
                            Name = "ingrOne",
                            Quantity = 1,
                        },
                        new Ingredient
                        {
                            Name = "ingrOne",
                            Quantity = 2,
                        }
                    }
                }
            };

            var result = ShoppingListCreator.CreateShoppingList(wantedRecipies, recipiesInCookbook);

            Assert.IsTrue(result.Count() == 2);
            Assert.AreEqual(result[0].Ingredients[0].Quantity, 5);
            Assert.AreEqual(result[1].Ingredients[1].Quantity, 14);

        }

        [TestMethod()]
        public void CreateShoppingList_EmptyInputLists()
        {
            List<ShoppingListDetailsDTO> wantedRecipies = new()
            {};

            List<Recipe> recipiesInCookbook = new()
            {};

            var result = ShoppingListCreator.CreateShoppingList(wantedRecipies, recipiesInCookbook);

            Assert.IsTrue(result.Count == 0);
        }

        //[TestMethod()]
        //public void AddDateToAdditionalItemLatestUseTest()
        //{
        //    UserCookBook user = new UserCookBook()
        //    {
        //        OwnerUsername = "test",
        //        PreviouslyAddedAdditonalItems = new List<AdditionalFoodItem>()
        //        {
        //            new AdditionalFoodItem
        //            {
        //                Name = "testItemOne",
        //            }
        //        }
        //    };

        //    SingleShopList newShopList = new()
        //    {
        //        AdditionalItems = new List<AdditionalFoodCurrentItem>()
        //        {
        //           new AdditionalFoodCurrentItem(){ 
        //           Name ="testItemOne"}
        //        }
        //    };

        //    var result = ShoppingListCreator.AddItemToAdditionalList(user, newShopList);

        //    Assert.IsTrue(result.PreviouslyAddedAdditonalItems[0].DatesWhenShopped.Count > 0);

        //}

        //[TestMethod]
        //public void AddDateToAdditionalItemLatestUse_AddNewItemToCookbookList()
        //{
        //    UserCookBook user = new UserCookBook()
        //    {
        //        OwnerUsername = "test",
        //    };

        //    SingleShopList newShopList = new()
        //    {
        //        AdditionalItems = new List<AdditionalFoodCurrentItem>()
        //        {
        //           new AdditionalFoodCurrentItem(){}
        //        }
        //    };

        //    var result = ShoppingListCreator.AddItemToAdditionalList(user, newShopList);

        //    Assert.IsTrue(result.PreviouslyAddedAdditonalItems[0].DatesWhenShopped.Count > 0);
        //}

        [TestMethod()]
        public void CheckIfItIsTimeToBuyAgainTest()
        {
            List<AdditionalFoodItem> mockData = new()
            {
                new AdditionalFoodItem
                {
                    Name = "item1",
                    DatesWhenShopped = new List<string>
                    {
                        new DateTime(2023, 2, 1).ToString("dd-MM-yyyy"),
                        new DateTime(2023, 3, 1).ToString("dd-MM-yyyy"),
                        new DateTime(2023, 4, 1).ToString("dd-MM-yyyy")
                    }
                },
                new AdditionalFoodItem
                {
                    Name = "item2",
                    DatesWhenShopped = new List<string>
                    {
                        new DateTime(2023, 4, 1).ToString("dd-MM-yyyy"),
                        new DateTime(2023, 4, 15).ToString("dd-MM-yyyy"),
                        new DateTime(2023, 4, 30).ToString("dd-MM-yyyy") // Dates are close, likely 'false' result
                    }
                },
                new AdditionalFoodItem
                {
                    Name = "item3",
                    DatesWhenShopped = new List<string>
                    {
                        new DateTime(2023, 4, 15).ToString("dd-MM-yyyy")
                    }
                }
            };

            DateTime testDate = new DateTime(2023, 05, 01); // Set a fixed date for testing

            // Act
            List<string> result = ShoppingListCreator.CheckIfItIsTimeToBuyAgain(mockData, testDate);


            Assert.IsTrue(result.Contains("item1"));
            Assert.IsFalse(result.Contains("item2"));
            Assert.IsFalse(result.Contains("item3"));
        }
    }
}