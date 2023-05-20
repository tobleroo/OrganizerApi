using Newtonsoft.Json;

namespace OrganizerApi.Calendar.Models
{
    public class CalendarTask
    {
        [JsonProperty(PropertyName = "_id")]
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
        public int timeToDoMinutes { get; set; }
        public CalendarTask(string title, int timeToDoMinutes)
        {
            this.Title = title;
            this.timeToDoMinutes = timeToDoMinutes;
            IsCompleted = false;
        }

    }
}
