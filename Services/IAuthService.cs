using SacViet.Models;
using SacViet.ViewModels;

namespace SacViet.Services
{
    public interface IAuthService
    {
        Task<(bool Success, string? Error, User? User)> RegisterAsync(RegisterViewModel model);
        Task<(bool Success, string? Error, User? User)> LoginAsync(LoginViewModel model);
        Task<User?> GetOrCreateGoogleUserAsync(string googleId, string email, string fullName, string? avatar);
        Task<User?> GetUserByIdAsync(int userId);
        Task UpdateLastLoginAsync(int userId);
        string HashPassword(string password);
        bool VerifyPassword(string password, string passwordHash);
    }
}
