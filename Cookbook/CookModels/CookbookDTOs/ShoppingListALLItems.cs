namespace OrganizerApi.Cookbook.CookModels.CookbookDTOs
{
    public class ShoppingListALLItems
    {
        public ShoppingList ShoppingList { get; set; }
        public List<string> EarlierAddedAdditionalItems { get; set; }
    }
}
