﻿using OrganizerApi.Diary.models;
using OrganizerApi.Diary.models.DiaryDTOs;

namespace OrganizerApi.Diary.DiaryServices
{
    public interface IDiaryService
    {
        Task<ProcessData> AddNewStory(string username, DiaryPost newPostData);
        Task<bool> CheckIfPasswordIsCreated(string username);
        Task<ProcessData> CreateDiaryAccount(string username, CreateDiaryDataDTO createDiaryDataDTO);
        Task<DiaryDTO> FetchDiary(string ownerName);
        Task<ProcessData> SignIn(string username, string passwordAttempt);
    }
}