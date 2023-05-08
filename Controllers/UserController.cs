using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OrganizerApi.models;
using OrganizerApi.models.DTOs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OrganizerApi.Controllers
{
    [Route("/user")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET: api/<ValuesController>
        //[HttpGet]
        //public User GetUser()
        //{
        //    var demoUser = new User
        //    {
        //        Id = 1,
        //        Name = "John",
        //        EmailAddress = "john@email.com",
        //        Password = "password"
        //    };

        //    return demoUser;
        //}

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public User? GetSepcificUser(int id)
        {

            
            if (id == 1)
            {
                var demoUser = new User
                {
                    Id = 1,
                    Name = "John",
                    EmailAddress = "deo@email.com",
                    Password = "password"
                };
                return demoUser;
            }
            else
            {
                return null;
            }

        }

        // POST api/<ValuesController>
        [HttpPost]
        public IActionResult CreateNewUser([FromBody] NewUserRequest newUserRequest)
        {
            //try
            //{
             

            //    return Ok();
            //}catch (Exception ex)
            //{
            //    return BadRequest($"user creating failed: {ex.Message}");
            //}
            
        }

    }
}
