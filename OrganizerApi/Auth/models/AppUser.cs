using Newtonsoft.Json;
using OrganizerApi.Calendar.Models;


namespace OrganizerApi.Auth.models
{
    [Serializable]
    public class AppUser
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }

        public List<CalendarDay>? Calendar { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
