namespace OrganizerApi.Cookbook.CookModels
{
    public class Recipe
    {
        public Guid Guid { get; set; } = Guid.NewGuid();
        public required string RecipeName { get; set; }
        public string? Description { get; set; } = "not set";
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
        public List<string> Steps { get; set; } = new List<string>();

        public int CookTime { get; set; } = 0;

        public string? Difficulty { get; set; } = "not set";
        public int TimesCooked { get; set; } = 0;
    }
}
