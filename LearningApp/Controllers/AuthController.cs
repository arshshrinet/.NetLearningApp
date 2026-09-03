using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LearningApp.Model;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using LearningApp.Services;
using Microsoft.AspNetCore.Authorization;

namespace LearningApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        public static User user = new User(); 
        [HttpPost("Register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            var user= await authService.RegisterUserAsync(request);
            if (user == null)
            {
                return BadRequest("UserName already exist.");
            }
            return Ok(user); 
        }

        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            var token = await authService.LoginUserAsync(request);
            if (token == null) {
                return BadRequest("Invalid username or password.");
            }
            return Ok(token);
        }

        [Authorize]
        [HttpGet]

        public ActionResult AutheticatedUserOnly()
        {
            return Ok("You are authenticated User.");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
       public ActionResult AdminOnlyEndPoint()
        {
            return Ok("You have admin role access.");
        }
    }
}
