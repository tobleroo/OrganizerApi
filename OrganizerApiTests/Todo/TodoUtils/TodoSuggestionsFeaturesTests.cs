using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrganizerApi.Todo.TodoUtils;
using OrganizerBlazor.Todo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizerApi.Todo.TodoUtils.Tests
{
    [TestClass()]
    public class TodoSuggestionsFeaturesTests
    {
        [TestMethod()]
        public void ConvertFrequencyToDaysTest()
        {
            // Arrange
            var activity = new TodoActivity { Title = "test", FrequencyAmount = 5, FrequencyType = FrequencyTypes.Days };

            // Act
            var result = TodoSuggestionsFeatures.ConvertFrequencyToDays(activity);

            // Assert
            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void GetDaysFromLastTimeDone_NeverCompleted_ReturnsMinusOne()
        {
            // Arrange
            var activity = new TodoActivity { Title = "test", DatesWhenCompleted = new List<string>() };

            // Act
            var result = TodoSuggestionsFeatures.GetDaysFromLastTimeDone(activity);

            // Assert
            Assert.AreEqual(-1, result);
        }

        [TestMethod]
        public void GetDaysFromLastTimeDone_ValidDate_ReturnsCorrectDays()
        {
            // Arrange
            var activity = new TodoActivity { Title = "test", DatesWhenCompleted = new List<string> { "01-04-2023" } };
            var expectedDays = (DateTime.Today - new DateTime(2023, 4, 1)).Days;

            // Act
            var result = TodoSuggestionsFeatures.GetDaysFromLastTimeDone(activity);

            // Assert
            Assert.AreEqual(expectedDays, result);
        }

        [TestMethod]
        public void GetSuggestions_WithActivities_ReturnsCorrectSuggestions()
        {
            // Arrange
            var categories = new List<TodoCategory>
            {
                new TodoCategory
                {
                    CategoryTitle = "Fitness",
                    Activities = new List<TodoActivity>
                    {
                        new TodoActivity
                        {
                            Title = "Running",
                            FrequencyAmount = 1,
                            FrequencyType = FrequencyTypes.Weeks,
                            DatesWhenCompleted = new List<string> { DateTime.Today.AddDays(-10).ToString("dd-MM-yyyy") }
                        },
                        new TodoActivity
                        {
                            Title = "Yoga",
                            FrequencyAmount = 2,
                            FrequencyType = FrequencyTypes.Days,
                            DatesWhenCompleted = new List<string> { DateTime.Today.AddDays(-1).ToString("dd-MM-yyyy") }
                        }
                    }
                },
                new TodoCategory
                {
                    CategoryTitle = "Learning",
                    Activities = new List<TodoActivity>
                    {
                        new TodoActivity
                        {
                            Title = "Read a book",
                            FrequencyAmount = 1,
                            FrequencyType = FrequencyTypes.Months,
                            DatesWhenCompleted = new List<string> { DateTime.Today.AddDays(-40).ToString("dd-MM-yyyy") }
                        },
                        new TodoActivity
                        {
                            Title = "Online Course",
                            FrequencyAmount = 1,
                            FrequencyType = FrequencyTypes.Years,
                            DatesWhenCompleted = new List<string>() // Never completed
                        }
                    }
                }
            };

            // Act
            var result = TodoSuggestionsFeatures.GetSuggestions(categories);

            // Assert
            Assert.IsTrue(result.SuggestedActivites.ContainsKey("Running"), "Running should be suggested");
            Assert.IsFalse(result.SuggestedActivites.ContainsKey("Yoga"), "Yoga should not be suggested");
            Assert.IsTrue(result.SuggestedActivites.ContainsKey("Read a book"), "Read a book should be suggested");
            Assert.IsTrue(result.ActivitesNeverDone.Contains("Online Course"), "Online Course should be in never done list");
        }
    }
}