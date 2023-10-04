namespace OrganizerBlazor.Models
{
    public class RecipeRequestEasyDTO
    {
        public int Portions { get; set; } = 1;
        public string Difficulty { get; set; }
        public string Category { get; set; }
        public int MaxCookTime { get; set; } = 120;

    }
}
