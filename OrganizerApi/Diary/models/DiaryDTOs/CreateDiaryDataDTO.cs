namespace OrganizerApi.Diary.models.DiaryDTOs
{
    public class CreateDiaryDataDTO
    {
        public string Password { get; set; }
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
    }
}
