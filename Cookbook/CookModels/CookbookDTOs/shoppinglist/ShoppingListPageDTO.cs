namespace OrganizerApi.Cookbook.CookModels.CookbookDTOs.shoppinglist
{
    public class ShoppingListPageDTO
    {
        public SingleShopList? SingleShopList { get; set; }
        public List<string>? AdditionalItems { get; set; }

        public List<string>? RecommendedAdditionalItems { get; set; } = new();
    }
}
