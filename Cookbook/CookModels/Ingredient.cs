namespace OrganizerApi.Cookbook.CookModels
{
    public class Ingredient
    {
        public Guid Guid { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public int? Quantity { get; set; }
        public string? Unit { get; set; }
    }
}
