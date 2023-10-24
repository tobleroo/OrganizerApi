﻿namespace OrganizerApi.Cookbook.CookModels
{
    public class UserCookBook
    {
        public Guid id { get; set; } = Guid.NewGuid();
        public required string OwnerUsername { get; set; }
        public List<Recipe> Recipes { get; set; } = new List<Recipe>();

        public List<ShoppingList> ShoppingList { get; set; } = new List<ShoppingList>();

        public List<string> PreviouslyAddedAdditonalItems { get; set; } = new();
    }
}
