using SacViet.Models;
using SacViet.Repositories;
using SacViet.ViewModels;
using System.Security.Cryptography;
using System.Text;

namespace SacViet.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<(bool Success, string? Error, User? User)> RegisterAsync(RegisterViewModel model)
        {
            if (await _userRepository.EmailExistsAsync(model.Email))
            {
                return (false, "Email đã được sử dụng", null);
            }

            var user = new User
            {
                Email = model.Email,
                PasswordHash = HashPassword(model.Password),
                FullName = model.FullName,
                RoleId = 3, // Reader role (default)
                IsEmailVerified = false,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            await _userRepository.CreateAsync(user);
            // Reload with Role populated
            var loaded = await _userRepository.GetByIdAsync(user.UserId);
            return (true, null, loaded);
        }

        public async Task<(bool Success, string? Error, User? User)> LoginAsync(LoginViewModel model)
        {
            var user = await _userRepository.GetByEmailAsync(model.Email);

            if (user == null)
            {
                return (false, "Email hoặc mật khẩu không đúng", null);
            }

            if (!user.IsActive.GetValueOrDefault())
            {
                return (false, "Tài khoản đã bị khóa", null);
            }

            if (string.IsNullOrEmpty(user.PasswordHash))
            {
                return (false, "Tài khoản này đăng nhập bằng Google", null);
            }

            if (!VerifyPassword(model.Password, user.PasswordHash))
            {
                return (false, "Email hoặc mật khẩu không đúng", null);
            }

            await UpdateLastLoginAsync(user.UserId);
            return (true, null, user);
        }

        public async Task<User?> GetOrCreateGoogleUserAsync(string googleId, string email, string fullName, string? avatar)
        {
            var user = await _userRepository.GetByGoogleIdAsync(googleId);
            if (user != null)
            {
                await UpdateLastLoginAsync(user.UserId);
                return user;
            }

            user = await _userRepository.GetByEmailAsync(email);
            if (user != null)
            {
                user.GoogleId = googleId;
                if (string.IsNullOrEmpty(user.Avatar))
                {
                    user.Avatar = avatar;
                }
                await _userRepository.UpdateAsync(user);
                await UpdateLastLoginAsync(user.UserId);
                // Reload with Role
                return await _userRepository.GetByIdAsync(user.UserId);
            }

            user = new User
            {
                Email = email,
                GoogleId = googleId,
                FullName = fullName,
                Avatar = avatar,
                RoleId = 3, // Reader role
                IsEmailVerified = true,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            await _userRepository.CreateAsync(user);
            return await _userRepository.GetByIdAsync(user.UserId);
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _userRepository.GetByIdAsync(userId);
        }

        public async Task UpdateLastLoginAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user != null)
            {
                user.LastLogin = DateTime.Now;
                await _userRepository.UpdateAsync(user);
            }
        }

        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput == passwordHash;
        }
    }
}

