using Microsoft.Azure.Cosmos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OrganizerApi.Auth.models;
using OrganizerApi.Auth.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OrganizerApi.Auth.Repository.Tests
{
    [TestClass()]
    public class UserRepositoryTests
    {

        [TestMethod()]
        public async Task GetUserTest()
        {
            // Arrange
            var mockCosmosClient = new Mock<CosmosClient>();
            var mockContainer = new Mock<Container>();
            var testUser = new AppUser { Id = Guid.NewGuid() };
            var testUserId = testUser.Id.ToString();

            var mockItemResponse = new Mock<ItemResponse<AppUser>>();
            mockItemResponse.SetupGet(r => r.Resource).Returns(testUser);

            // Setup for ReadItemAsync call
            mockContainer.Setup(c => c.ReadItemAsync<AppUser>(It.IsAny<string>(), It.IsAny<PartitionKey>(), It.IsAny<ItemRequestOptions>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(mockItemResponse.Object);

            var mockDatabase = new Mock<Database>();
            mockDatabase.Setup(d => d.GetContainer(It.IsAny<string>())).Returns(mockContainer.Object);
            mockCosmosClient.Setup(c => c.GetDatabase(It.IsAny<string>())).Returns(mockDatabase.Object);

            // Create repository with mocked CosmosClient, DatabaseId, and ContainerId
            var repository = new UserRepository(mockCosmosClient.Object, "DatabaseId", "ContainerId");

            // Act
            var result = await repository.GetUser(testUserId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(testUser, result);
        }

        //[TestMethod]
        //public async Task GetUserByUsername_HandlesCosmosException()
        //{
        //    // Arrange
        //    var mockContainer = new Mock<Container>();
        //    var testUsername = "testUsername";

        //    var cosmosException = new CosmosException("Error occurred", HttpStatusCode.BadRequest, 0, "TestError", 0);
        //    var mockFeedIterator = new Mock<FeedIterator<AppUser>>();

        //    // Set up the GetItemQueryIterator call
        //    mockContainer.Setup(c => c.GetItemQueryIterator<AppUser>(
        //                             It.IsAny<string>(),
        //                             null,
        //                             It.IsAny<QueryRequestOptions>()))
        //                 .Returns(mockFeedIterator.Object);

        //    mockFeedIterator.Setup(x => x.ReadNextAsync(It.IsAny<CancellationToken>()))
        //                    .ThrowsAsync(cosmosException);

        //    var repository = new UserRepository(mockContainer.Object);

        //    // Act & Assert
        //    await Assert.ThrowsExceptionAsync<CosmosException>(async () =>
        //    {
        //        await repository.GetUserByUsername(testUsername);
        //    });
        //}

        [TestMethod()]
        public async Task SaveNewUserTest()
        {
            // Arrange
            var mockCosmosClient = new Mock<CosmosClient>();
            var mockContainer = new Mock<Container>();
            var newUser = new AppUser { Id = Guid.NewGuid() };

            var mockItemResponse = new Mock<ItemResponse<AppUser>>();
            mockItemResponse.SetupGet(r => r.Resource).Returns(newUser);

            mockContainer.Setup(c => c.CreateItemAsync(It.IsAny<AppUser>(), It.IsAny<PartitionKey>(), null, CancellationToken.None))
                         .ReturnsAsync(mockItemResponse.Object);

            var mockDatabase = new Mock<Database>();
            mockDatabase.Setup(d => d.GetContainer(It.IsAny<string>())).Returns(mockContainer.Object);
            mockCosmosClient.Setup(c => c.GetDatabase(It.IsAny<string>())).Returns(mockDatabase.Object);

            var repository = new UserRepository(mockCosmosClient.Object, "DatabaseId", "ContainerId");

            // Act
            var result = await repository.SaveNewUser(newUser);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [TestMethod()]
        public async Task UpdateUserTest()
        {
            // Arrange
            var mockCosmosClient = new Mock<CosmosClient>();
            var mockContainer = new Mock<Container>();
            var existingUser = new AppUser { Id = Guid.NewGuid() };

            var mockItemResponse = new Mock<ItemResponse<AppUser>>();
            mockItemResponse.SetupGet(r => r.Resource).Returns(existingUser);

            mockContainer.Setup(c => c.UpsertItemAsync(It.IsAny<AppUser>(), It.IsAny<PartitionKey>(), It.IsAny<ItemRequestOptions>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(mockItemResponse.Object);

            var mockDatabase = new Mock<Database>();
            mockDatabase.Setup(d => d.GetContainer(It.IsAny<string>())).Returns(mockContainer.Object);
            mockCosmosClient.Setup(c => c.GetDatabase(It.IsAny<string>())).Returns(mockDatabase.Object);

            var repository = new UserRepository(mockCosmosClient.Object, "DatabaseId", "ContainerId");

            // Act
            var result = await repository.UpdateUser(existingUser);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(existingUser, result);
        }

        [TestMethod()]
        public async Task GetUserByEmailTest()
        {
            // Arrange
            var mockCosmosClient = new Mock<CosmosClient>();
            var mockContainer = new Mock<Container>();
            var testEmail = "test@example.com";
            var testUser = new AppUser { EmailAddress = testEmail };

            var mockFeedIterator = new Mock<FeedIterator<AppUser>>();
            var mockFeedResponse = new Mock<FeedResponse<AppUser>>();
            mockFeedResponse.Setup(_ => _.GetEnumerator()).Returns(new List<AppUser> { testUser }.GetEnumerator());
            mockFeedResponse.Setup(_ => _.Resource).Returns(new List<AppUser> { testUser });

            mockFeedIterator.Setup(_ => _.ReadNextAsync(It.IsAny<CancellationToken>()))
                            .ReturnsAsync(mockFeedResponse.Object);
            mockFeedIterator.Setup(_ => _.HasMoreResults).Returns(false);

            mockContainer.Setup(c => c.GetItemQueryIterator<AppUser>(
                                     It.IsAny<QueryDefinition>(),
                                     null,
                                     It.IsAny<QueryRequestOptions>()))
                         .Returns(mockFeedIterator.Object);

            var mockDatabase = new Mock<Database>();
            mockDatabase.Setup(d => d.GetContainer(It.IsAny<string>())).Returns(mockContainer.Object);
            mockCosmosClient.Setup(c => c.GetDatabase(It.IsAny<string>())).Returns(mockDatabase.Object);

            var repository = new UserRepository(mockCosmosClient.Object, "DatabaseId", "ContainerId");

            // Act
            var result = await repository.GetUserByEmail(testEmail);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(testUser.EmailAddress, result.EmailAddress);
        }

        [TestMethod()]
        public async Task CheckIfUsernameExistsTest()
        {
            // Arrange
            var mockCosmosClient = new Mock<CosmosClient>();
            var mockContainer = new Mock<Container>();
            var testUsername = "testUsername";
            var users = new List<AppUser> { new AppUser() }; // Simulating a user found

            var mockFeedIterator = new Mock<FeedIterator<AppUser>>();
            var mockFeedResponse = new Mock<FeedResponse<AppUser>>();
            mockFeedResponse.Setup(_ => _.GetEnumerator()).Returns(users.GetEnumerator());
            mockFeedIterator.Setup(_ => _.ReadNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(mockFeedResponse.Object);
            mockFeedIterator.Setup(_ => _.HasMoreResults).Returns(false);

            mockContainer.Setup(c => c.GetItemQueryIterator<AppUser>(
                                     It.IsAny<QueryDefinition>(),
                                     null,
                                     It.IsAny<QueryRequestOptions>()))
                         .Returns(mockFeedIterator.Object);

            var mockDatabase = new Mock<Database>();
            mockDatabase.Setup(d => d.GetContainer(It.IsAny<string>())).Returns(mockContainer.Object);
            mockCosmosClient.Setup(c => c.GetDatabase(It.IsAny<string>())).Returns(mockDatabase.Object);

            var repository = new UserRepository(mockCosmosClient.Object, "DatabaseId", "ContainerId");

            // Act
            var result = await repository.CheckIfUsernameExists(testUsername);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public async Task CheckIfEmailExistsTest()
        {
            // Arrange
            var mockCosmosClient = new Mock<CosmosClient>();
            var mockContainer = new Mock<Container>();
            var testEmail = "test@example.com";
            var users = new List<AppUser>() { new AppUser { EmailAddress = testEmail } };

            var mockFeedIterator = new Mock<FeedIterator<AppUser>>();
            var mockFeedResponse = new Mock<FeedResponse<AppUser>>();
            mockFeedResponse.Setup(_ => _.GetEnumerator()).Returns(users.GetEnumerator());
            mockFeedIterator.Setup(_ => _.ReadNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(mockFeedResponse.Object);
            mockFeedIterator.Setup(_ => _.HasMoreResults).Returns(false);

            mockContainer.Setup(c => c.GetItemQueryIterator<AppUser>(
                                     It.IsAny<QueryDefinition>(),
                                     null,
                                     It.IsAny<QueryRequestOptions>()))
                         .Returns(mockFeedIterator.Object);

            var mockDatabase = new Mock<Database>();
            mockDatabase.Setup(d => d.GetContainer(It.IsAny<string>())).Returns(mockContainer.Object);
            mockCosmosClient.Setup(c => c.GetDatabase(It.IsAny<string>())).Returns(mockDatabase.Object);

            var repository = new UserRepository(mockCosmosClient.Object, "DatabaseId", "ContainerId");

            // Act
            var result = await repository.CheckIfEmailExists(testEmail);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public async Task CheckEmailAndUsernameExistsTest()
        {
            // Arrange
            var mockCosmosClient = new Mock<CosmosClient>();
            var mockContainer = new Mock<Container>();
            var testUsername = "testUsername";
            var testEmail = "test@example.com";

            var mockFeedIterator = new Mock<FeedIterator<int>>();

            // Set up the GetItemQueryIterator call
            mockContainer.Setup(c => c.GetItemQueryIterator<int>(
                                     It.IsAny<QueryDefinition>(),
                                     null,
                                     It.IsAny<QueryRequestOptions>()))
                         .Returns(mockFeedIterator.Object);

            // Mock response for when the email or username exists
            var countList = new List<int> { 1 }; // Simulating a count of 1
            var mockFeedResponse = new Mock<FeedResponse<int>>();
            mockFeedResponse.Setup(_ => _.GetEnumerator()).Returns(countList.GetEnumerator());
            mockFeedIterator.Setup(_ => _.ReadNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(mockFeedResponse.Object);
            mockFeedIterator.Setup(_ => _.HasMoreResults).Returns(false);

            var mockDatabase = new Mock<Database>();
            mockDatabase.Setup(d => d.GetContainer(It.IsAny<string>())).Returns(mockContainer.Object);
            mockCosmosClient.Setup(c => c.GetDatabase(It.IsAny<string>())).Returns(mockDatabase.Object);

            var repository = new UserRepository(mockCosmosClient.Object, "DatabaseId", "ContainerId");

            // Act
            var result = await repository.CheckEmailAndUsernameExists(testEmail, testUsername);

            // Assert
            Assert.IsTrue(result);
        }
    }
}