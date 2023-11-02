﻿using OrganizerApi.Cookbook.CookModels;
using OrganizerApi.Cookbook.CookModels.CookbookDTOs;
using OrganizerApi.Cookbook.CookModels.CookbookDTOs.shoppinglist;

namespace OrganizerApi.Cookbook.CookRepository
{
    public interface ICookBookRepository
    {

        Task<UserCookBook> GetCookBook(string id);

        Task<UserCookBook> SaveNewCookBook(UserCookBook cookbook);
        Task<bool> UpdateCookBook(UserCookBook cookbook);

        Task<SingleShopList?> GetShoppingList(string username);

        Task<bool> UpsertAdditionalItemsShoppingList(string username, List<string> shoppinglistToUpdate);
    }
}
