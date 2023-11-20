using Microsoft.Azure.Cosmos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OrganizerApi.Cookbook.CookModels;
using OrganizerApi.Cookbook.CookRepository;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [TestMethod()]
        public void GetCookBookTest()
        {
            Mock mockContainer = new Mock<Container>();
            var mockFeedIterator = new Mock<FeedIterator<UserCookBook>>();
            //mockContainer.Setup(c => c.GetItemQueryIterator<UserCookBook>(It.IsAny<QueryDefinition>(), null, null))
            //             .Returns(mockFeedIterator.Object);


        }

        [TestMethod()]
        public void UpdateCookBookTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void GetShoppingListTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void FetchUserCookbookIdTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void FetchRecipiesOverviewTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void FetchOneRecipeTest()
        {
            throw new NotImplementedException();
        }
    }
}