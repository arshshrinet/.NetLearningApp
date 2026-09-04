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
        public async Task<ActionResult<TokenResponseDto>> Register(UserDto request)
        {
            var user = await authService.RegisterUserAsync(request);
            if (user == null)
            {
                return BadRequest("UserName already exist.");
            }
            return Ok(user);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<TokenResponseDto>> Login(UserDto request)
        {
            var result = await authService.LoginUserAsync(request);
            if (result == null)
            {
                return BadRequest("Invalid username or password.");
            }
            return Ok(result);
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

        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto refreshTokenRequest)
        {
            var result = await authService.RefreshTokensAsync(refreshTokenRequest);
            if (result is null || result.AccessToken is null || result.RefreshToken is null)
            {
                return Unauthorized("Invalid refresh token.");
            }
            return Ok(result);
        }
    }
}
