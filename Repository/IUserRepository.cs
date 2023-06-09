﻿using OrganizerApi.models;

namespace OrganizerApi.Repository
{
    public interface IUserRepository
    {

        Task<AppUser> GetUser(string id);
        Task<AppUser> SaveNewUser(AppUser user);

        Task<AppUser> GetUserByUsername(string username);

        Task<AppUser> UpdateUser(AppUser user);

    }
}
