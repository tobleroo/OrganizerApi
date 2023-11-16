using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace OrganizerApi.Calendar.Models
{
    public class CalendarEvent
    {

        [JsonProperty(PropertyName = "_id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        public string EventTime { get; set; }

        public CalendarEvent(string title,string desc, string timeOfEvent )
        {
            Title = title;
            Description = desc;
            EventTime = timeOfEvent;
        }

        // Add a parameterless constructor for deserialization
        public CalendarEvent()
        {
        }

    }
}
