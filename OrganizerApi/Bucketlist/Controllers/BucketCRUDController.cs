using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrganizerApi.Bucketlist.Models;
using OrganizerApi.Bucketlist.Services;
using System.Security.Claims;

namespace OrganizerApi.Bucketlist.Controllers
{
    [Route("bucketlist")]
    [ApiController]
    [Authorize]
    public class BucketCRUDController : Controller
    {

        private readonly IBucketlistService _bucketlistService;

        public BucketCRUDController(IBucketlistService bucketlistService) {
            _bucketlistService = bucketlistService;
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetCookBook()
        {
            var name = User.FindFirstValue(ClaimTypes.Name);
            try
            {
                var bucketlist = await _bucketlistService.GetBucketlist(name);
                return Ok(bucketlist);
            }
            catch (Exception ex)
            {
                return BadRequest("something went wrong -> " + ex.Message);
            }
        }

        [HttpPost("save-achievement")]
        public async Task<IActionResult> SaveBucketlistData([FromBody] Achievement achievement)
        {
            var name = User.FindFirstValue(ClaimTypes.Name);
            try
            {
                var success = await _bucketlistService.SaveNewAchievement(name, achievement);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("something went wrong -> " + ex.Message);
            }
        }
    }
}
