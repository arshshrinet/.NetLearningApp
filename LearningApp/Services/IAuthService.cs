using LearningApp.Model;

namespace LearningApp.Services
{
    public interface IAuthService
    {
        Task<User?> RegisterUserAsync(UserDto request);
        Task<string?> LoginUserAsync(UserDto request);
    }
}