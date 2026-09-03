using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LearningApp.Model;
using LearningApp.Repository;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace LearningApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IConfiguration configuration) : ControllerBase
    {
        public static User user = new User(); 
        [HttpPost("Register")]
        public ActionResult<User> Register(UserDto request)
        {
            var hashedPassword = new PasswordHasher<User>()
                .HashPassword(user, request.Password); 
            user.UserName = request.UserName; 
            user.PasswordHash = hashedPassword; 
            return Ok(user); 
        }

        [HttpPost("Login")]
        public ActionResult<string> Login(UserDto request)
        {
            if(user.UserName != request.UserName)
            {
                return BadRequest("User not found.");
            }

            if(new PasswordHasher<User>().VerifyHashedPassword(user,user.PasswordHash, request.Password)== PasswordVerificationResult.Failed)
            {
                return BadRequest("Invalid password.");
            }

            string token = CreateToken(user);
            return Ok(token);
        }

        private string CreateToken(User user)
        {
            var claim = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:token")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("AppSettings:issuer"),
                audience: configuration.GetValue<string>("AppSettings:audience"),
                claims: claim,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
