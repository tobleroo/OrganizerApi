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

        public CalendarEvent(string title, string desc = "not set")
        {
            Title = title;
            Description = desc;
        }

        // Add a parameterless constructor for deserialization
        public CalendarEvent()
        {
        }

    }
}
