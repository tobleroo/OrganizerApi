namespace OrganizerApi.Cookbook.CookModels
{
    public class ShoppingList
    {
        public Guid Guid { get; set; } = Guid.NewGuid();
        public required string ListName { get; set; }
        public List<RecipeShoppingListData>? RecipeToShoppingListData { get; set; } 
        public List<string> AdditionalItems { get; set; } = new();
    }
}
