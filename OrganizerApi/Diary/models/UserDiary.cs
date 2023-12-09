namespace OrganizerApi.Diary.models
{
    public class UserDiary
    {
        public string id { get; set; } = Guid.NewGuid().ToString();
        public required string OwnerUsername { get; set; }

        public List<DiaryPost> Posts { get; set; } = new List<DiaryPost>();

        public string OwnerHomeCountry { get; set; } = "";
        public string OwnerHomeTown { get; set; } = "";

        public string DiaryPassword { get; set; } = "";

    }
}
