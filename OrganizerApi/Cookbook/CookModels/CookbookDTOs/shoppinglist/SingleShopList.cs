namespace OrganizerApi.Cookbook.CookModels.CookbookDTOs.shoppinglist
{
    public class SingleShopList
    {
        public string ListName { get; set; } = "Shopping list";
        public List<SingleShopListRecipe> SingleShopListRecipes { get; set; } = new List<SingleShopListRecipe>();

        public List<AdditionalFoodCurrentItem> AdditionalItems { get; set; } = new List<AdditionalFoodCurrentItem>();

    }
}
