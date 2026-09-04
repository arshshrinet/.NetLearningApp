using LearningApp.Model;

namespace LearningApp.Services
{
    public interface IAuthService
    {
        Task<User?> RegisterUserAsync(UserDto request);
        Task<TokenResponseDto?> LoginUserAsync(UserDto request);
        Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request);
    }
}