using OrganizerApi.Cookbook.CookModels.CookbookDTOs.shoppinglist;

namespace OrganizerApi.Cookbook.CookModels
{
    public class UserCookBook
    {
        public Guid id { get; set; } = Guid.NewGuid();
        public required string OwnerUsername { get; set; }
        public List<Recipe> Recipes { get; set; } = new List<Recipe>();

        public SingleShopList ShoppingList { get; set; } = new SingleShopList();

        public List<AdditionalFoodItem> PreviouslyAddedAdditonalItems { get; set; } = new();

        public List<string> GetPreviouslyAddedItemsAsListOfStrings()
        {
            var list = new List<string>();

            foreach (var item in PreviouslyAddedAdditonalItems)
            {
                list.Add(item.Name);
            }

            return list;
        }
    }
}
