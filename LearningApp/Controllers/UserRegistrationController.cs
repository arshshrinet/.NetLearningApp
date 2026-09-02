using LearningApp.Model;
using Microsoft.AspNetCore.Mvc;

namespace LearningApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserRegistrationController : ControllerBase
    {  

        [Route("CreateUser")]
        [HttpPost]
        public async Task<IActionResult> CreateUser(UserRegistrationModel userRegistrationModel)
        {
            return Created();
        }

        [Route("Users/{id}")]
        [HttpGet]
        public IEnumerable<UserRegistrationModel> Users()
        {
            return Enumerable.Range(1, 2).Select(index => new UserRegistrationModel
            {
                Name = "TestUser",
                Email = "TestUser@email.com",
                PhoneNumber = Random.Shared.Next(-20, 55).ToString(),
                Address = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        private static readonly string[] Summaries =
       [
           "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
       ];
    }
}
