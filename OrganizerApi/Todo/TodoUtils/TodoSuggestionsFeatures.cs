using Microsoft.AspNetCore.Http;
using OrganizerApi.Todo.Models.DTOs;
using OrganizerBlazor.Todo.Models;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OrganizerApi.Todo.TodoUtils
{
    public class TodoSuggestionsFeatures
    {

        public static int ConvertFrequencyToDays(TodoActivity activity)
        {

            int frequencyToDays = activity.FrequencyType switch
            {
                FrequencyTypes.Days => 1,
                FrequencyTypes.Weeks => activity.FrequencyAmount * 7,
                FrequencyTypes.Months => activity.FrequencyAmount * 30,
                FrequencyTypes.Years => activity.FrequencyAmount * 365,
                _ => 0
            };

            return activity.FrequencyAmount * frequencyToDays;
        }

        public static int GetDaysFromLastTimeDone(TodoActivity activity)
        {
            //get the date string from the last time it was done
            string lastTimeDoneDate = activity.DatesWhenCompleted.LastOrDefault();

            // return -1 to know the activity has never been completed
            if (lastTimeDoneDate == null) return -1;

            //convert to date object
            var dateObj = DateTime.ParseExact(lastTimeDoneDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);

            //get todays date and check how long ago it was
            DateTime today = DateTime.Today;

            // Calculate the difference in days
            TimeSpan difference = today - dateObj;
            return (int)difference.TotalDays;
        }

        public static TodoSuggestionsDTO GetSuggestions(List<TodoCategory> todoCategories)
        {
            var activityAndDaysSince = new Dictionary<string, int>();
            var tasksNeverCompleted = new List<string>();

            foreach (var category in todoCategories)
            {
                foreach (var activity in category.Activities)
                {
                    int shouldBeDoneAfter = ConvertFrequencyToDays(activity);
                    int daysSinceLast = GetDaysFromLastTimeDone(activity);

                    if (daysSinceLast == -1) tasksNeverCompleted.Add(activity.Title);

                    if (daysSinceLast >= shouldBeDoneAfter) activityAndDaysSince.Add(activity.Title, daysSinceLast);
                }
            }

            //sort the dictionary, top should be prio one
            //the one thats been the ongest since done
            TodoSuggestionsDTO todoSuggestionsDTO = new TodoSuggestionsDTO()
            {
                SuggestedActivites = activityAndDaysSince
                    .OrderByDescending(pair => pair.Value)
                    .ToDictionary(pair => pair.Key, pair => pair.Value),
                ActivitesNeverDone = tasksNeverCompleted
            };
        
            return todoSuggestionsDTO;
        }

        
    }
}
