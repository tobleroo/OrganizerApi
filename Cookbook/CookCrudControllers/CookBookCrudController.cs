using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using OrganizerApi.Cookbook.CookServices;
using System.Security.Claims;

namespace OrganizerApi.Cookbook.CookCrudControllers
{

    [Route("/cookbook")]
    [ApiController]
    [Authorize]
    public class CookBookCrudController : Controller
    {
        private readonly ICookBookService _cookBookService;

        public CookBookCrudController(ICookBookService cookBookService)
        {
            _cookBookService = cookBookService;
        }

        [HttpGet("get-cookbook")]
        public async Task<IActionResult> GetCookBook()
        {
            var name = User.FindFirstValue(ClaimTypes.Name);
            var cookBook = await _cookBookService.GetCookBook(name);
            return Ok(cookBook);
        }
    }
}
