namespace OrganizerApi.Cookbook.CookModels
{
    public class RecipeShoppingListData
    {
        public string RecipeName { get; set; } = "not set";
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
    }
}
