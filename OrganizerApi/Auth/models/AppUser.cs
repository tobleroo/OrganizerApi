using Newtonsoft.Json;
using OrganizerApi.Diary.models;
using OrganizerBlazor.Todo.Models;


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

        public UserDiary Diary { get; set; } = new UserDiary() { OwnerUsername = "not set"};

        public TodoDocument TodoDoc { get; set; } = new TodoDocument() { Owner = "not set!" };

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
