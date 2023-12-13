﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using OrganizerApi.Diary.DiaryUtils;
using OrganizerApi.Diary.models;
using OrganizerApi.Diary.models.DiaryDTOs;
using OrganizerApi.Diary.Repository;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace OrganizerApi.Diary.DiaryServices
{
    public class DiaryService : IDiaryService
    {

        private IDiaryRepository _diaryRepo;

        public DiaryService(IDiaryRepository diaryRepository) {
            _diaryRepo = diaryRepository;
        }

        public async Task<bool> CheckDiaryPassword(string username, string password)
        {
            //get the encrypted password from diary to that username

            // decrypt and compare to attmepted password

            //return if a match

            throw new NotImplementedException();
        }

        public async Task<DiaryDTO> FetchDiary(string ownerName) {
            
            var diary = await _diaryRepo.GetDiary(ownerName);

            //decrypt content and title of all posts
            foreach (var diaryPost in diary.Posts) {
                diaryPost.Title = DiaryEncryption.DecryptContent(diaryPost.Title);
                diaryPost.Content = DiaryEncryption.DecryptContent(diaryPost.Content);
            }

            //turn into DTO to not send password to client
            return new DiaryDTO() {
                OwnerName = diary.OwnerUsername,
                Posts = diary.Posts,
                OwnerHomeCountry = diary.OwnerHomeCountry,
                OwnerHomeTown = diary.OwnerHomeTown,
            };
        }

        public async Task<ProcessData> DeleteOnePost(string username, string postId)
        {
            var process = new ProcessData() { IsValid = false, Message="Could not delete post!" };
            //get the diary fro musername
            var diary = await _diaryRepo.GetDiary(username);

            //run through all posts and remove where id is same
            //go backwards to remove while in loop
            foreach (var post in diary.Posts)
            {
                if(post.Id == postId)
                {
                    diary.Posts.Remove(post);
                    break;
                }
            }

            //save diary
            var couldDelete = await _diaryRepo.UpsertDiary(diary);
            if(couldDelete) process.IsValid = true; process.Message = "Post deleted!";

            return process;
        }

        public async Task<ProcessData> CreateDiaryAccount(string username, CreateDiaryDataDTO createDiaryDataDTO)
        {
            // check that password is legit in requirements
            var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[A-Za-z\d]{8,}$");
            var isLegitPassword = regex.IsMatch(createDiaryDataDTO.Password);

            if (!isLegitPassword)
                return new ProcessData() { IsValid = false, Message="Password has not met requirements!" };


            var diary = await _diaryRepo.GetDiary(username);

            diary.OwnerHomeTown = createDiaryDataDTO.City;
            diary.OwnerHomeCountry = createDiaryDataDTO.Country;

            //encrypt password
            var passwordHasher = new PasswordHasher<object>();
            string hashedPassword = passwordHasher.HashPassword(null, createDiaryDataDTO.Password);

            diary.DiaryPassword = hashedPassword;

            // save the data and return success or not
            var success = await _diaryRepo.UpsertDiary(diary);
            if (success)
                return new ProcessData() { IsValid = true, Message = "successfully created diary account!" };

            return new ProcessData() { IsValid = false, Message="Something went wrong!"};

        }

        public async Task<bool> CheckIfPasswordIsCreated(string username)
        {
            //check diary password from username in diary
            var userDiary = await _diaryRepo.GetDiary(username);

            //if empty, it has not bee nset and client must do so
            if (userDiary.DiaryPassword.IsNullOrEmpty())
            {
                return false;
            }
            else return true;

        }

        public async Task<ProcessData> SignIn(string username, string passwordAttempt)
        {
            var diaryPassword = await _diaryRepo.FetchPassword(username);

            if (diaryPassword.IsNullOrEmpty()) return new ProcessData() { IsValid = false, Message = "no password has been set!"};

            //unencrypt and compare
            var passwordHasher = new PasswordHasher<object>();
            var result = passwordHasher.VerifyHashedPassword(null, diaryPassword, passwordAttempt);
            bool passwordIsMatch = result == PasswordVerificationResult.Success;

            var resultProcess = new ProcessData() { IsValid = passwordIsMatch };
            if (passwordIsMatch) resultProcess.Message = "successful sign in!";
            else resultProcess.Message = "Wrong password!";

            return resultProcess;
        }

        public async Task<ProcessData> AddNewStory(string username, DiaryPost newPostData)
        {

            //encrypt diary content
            newPostData.Content = DiaryEncryption.EncryptContent(newPostData.Content);
            newPostData.Title = DiaryEncryption.EncryptContent(newPostData.Title);

            // save to db
            var diaryId = await _diaryRepo.GetDocumentIdByUsernameAsync(username);
            return await _diaryRepo.PatchNewStory(diaryId, newPostData);

        }

        public async Task<ProcessData> UpdatePost(string username, DiaryPost newPostData)
        {

            ProcessData process = new() { IsValid = false , Message = "could not update post!"};

            //encrypt new content
            var titleEncrypted = DiaryEncryption.EncryptContent(newPostData.Title);
            var contentEncrypted = DiaryEncryption.EncryptContent(newPostData.Content);

            // resave the post 
            var diary = await _diaryRepo.GetDiary(username);
            DiaryPost postToUpdate = diary.Posts.First(p => p.Id == newPostData.Id);

            postToUpdate.Content = contentEncrypted;
            postToUpdate.Title = titleEncrypted;
            postToUpdate.LatestRevisedDate = newPostData.LatestRevisedDate;

            var successResave = await _diaryRepo.UpsertDiary(diary);
            if (successResave) process.IsValid = true; process.Message = "successfully updated post!";

            return process;
        }
        public async Task<ProcessData> UpdateAddress(string username, DiaryDTO dto)
        {
            var processData = new ProcessData() { Message = " could not change address!", IsValid = false };
            var diary = await _diaryRepo.GetDiary(username);

            diary.OwnerHomeTown = dto.OwnerHomeTown;
            diary.OwnerHomeCountry = dto.OwnerHomeCountry;

            var success = await _diaryRepo.UpsertDiary(diary);

            if (success) processData.Message = "Address changed!"; processData.IsValid = true;
            return processData;
        }
    }
}
