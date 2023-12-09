using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrganizerApi.Diary.DiaryServices;
using OrganizerApi.Diary.models;
using OrganizerApi.Diary.models.DiaryDTOs;
using System.Security.Claims;

namespace OrganizerApi.Diary.DiaryControllers
{

    [Route("/diary")]
    [ApiController]
    [Authorize]
    public class DiaryCrudController : Controller
    {

        private IDiaryService _diaryService;

        public DiaryCrudController(IDiaryService diaryService)
        {
            _diaryService = diaryService;
        }

        [HttpPost("add-story")]
        public async Task<IActionResult> AddStoryToDiary([FromBody] DiaryPost newPost)
        {
            var name = User.FindFirstValue(ClaimTypes.Name);
            var process = await _diaryService.AddNewStory(name, newPost);
            return Ok(process);
        }


        [HttpGet("get-diary")]
        public async Task<IActionResult> GetCookBook()
        {
            var name = User.FindFirstValue(ClaimTypes.Name);
            try
            {
                var diary = await _diaryService.FetchDiary(name);
                return Ok(diary);
            }
            catch (Exception ex)
            {
                return BadRequest("something went wrong -> " + ex.Message);
            }
        }

        [HttpGet("check-if-new")]
        public async Task<IActionResult> SeeDiaryCreated()
        {
            var name = User.FindFirstValue(ClaimTypes.Name);
            try
            {
                var createdOrNot = await _diaryService.CheckIfPasswordIsCreated(name);
                return Ok(createdOrNot);
            }
            catch (Exception ex)
            {
                return BadRequest("something went wrong -> " + ex.Message);
            }
        }

        [HttpPost("create-account")]
        public async Task<IActionResult> CreateDiaryAccount(CreateDiaryDataDTO createDiaryDataDTO)
        {
            var name = User.FindFirstValue(ClaimTypes.Name);
            var success = await _diaryService.CreateDiaryAccount(name, createDiaryDataDTO);

            return Ok(success);
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignInDiary([FromBody] SignInDTO signInDTO)
        {
            var name = User.FindFirstValue(ClaimTypes.Name);
            var SignInProcessData = await _diaryService.SignIn(name, signInDTO.Password);
            return Ok(SignInProcessData);
        }
    }
        
}
