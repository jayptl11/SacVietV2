using SacViet.Models;

namespace SacViet.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(int userId);
        Task<User?> GetByGoogleIdAsync(string googleId);
        Task<User> CreateAsync(User user);
        Task UpdateAsync(User user);
        Task<bool> EmailExistsAsync(string email);
    }
}
