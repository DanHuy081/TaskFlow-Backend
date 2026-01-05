using CoreEntities.Model.DTOs;
using CoreEntities.Model;
using LogicBusiness.Repository;
using LogicBusiness.UseCase;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using LogicBusiness.Helpers;

namespace LogicBusiness.Service
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _repo;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;

        public AuthService(IUserRepository repo, IConfiguration config, IEmailService emailService)
        {
            _repo = repo;
            _config = config;
            _emailService = emailService;

        }

        // REGISTER -----------------------
        public async Task<bool> RegisterAsync(RegisterDto dto)
        {
            if (await _repo.GetByUsernameAsync(dto.Username) != null)
                throw new Exception("Username already exists.");

            CreatePasswordHash(dto.Password, out byte[] hash, out byte[] salt);

            var user = new UserFL
            {
                UserId = Guid.NewGuid().ToString(),
                Username = dto.Username,
                FullName = dto.FullName,
                Email = dto.Email,
                Role = "Member",
                PasswordHash = hash,
                PasswordSalt = salt,
                DateCreated = DateTime.UtcNow
            };

            await _repo.CreateAsync(user);
            return true;
        }

        // LOGIN ---------------------------
        public async Task<string?> LoginAsync(LoginDto dto)
        {
            var user = await _repo.GetByUsernameAsync(dto.Username);
            if (user == null) return null;

            if (!VerifyPasswordHash(dto.Password, user.PasswordHash, user.PasswordSalt))
                return null;

            return CreateJwtToken(user);
        }

        // HASHING -------------------------
        private void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            salt = hmac.Key;
            hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPasswordHash(string password, byte[] hash, byte[] salt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512(salt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(hash);
        }

        // JWT ------------------------------
        private string CreateJwtToken(UserFL user)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.UserId),
            new Claim(ClaimTypes.Name, user.Username),

            new Claim(ClaimTypes.Role, user.Role ?? "Member")
        };

            var key = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(_config["Jwt:Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                expires: DateTime.UtcNow.AddDays(7),
                claims: claims,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task ForgotPasswordAsync(string email)
        {
            var user = await _repo.GetByEmailAsync(email);
            if (user == null) return; // Không tiết lộ email tồn tại hay không

            user.ResetPasswordToken = Guid.NewGuid();
            user.ResetPasswordExpire = DateTime.UtcNow.AddMinutes(15);
            user.IsResetPasswordUsed = false;

            await _repo.UpdateAsync(user);

            var resetLink = $"https://your-fe-domain/reset-password?token={user.ResetPasswordToken}";

            await _emailService.SendAsync(
                user.Email,
                "Reset your password",
                $"Click vào link để đặt lại mật khẩu: {resetLink}"
            );
        }

        public async Task ResetPasswordAsync(Guid token, string newPassword)
        {
            var user = await _repo.GetByResetTokenAsync(token);
            if (user == null)
                throw new Exception("Token không hợp lệ hoặc đã hết hạn");

            // Hash password
            PasswordHasher.CreatePasswordHash(
                newPassword,
                out byte[] hash,
                out byte[] salt);

            user.PasswordHash = hash;
            user.PasswordSalt = salt;
            user.IsResetPasswordUsed = true;
            user.ResetPasswordToken = null;
            user.ResetPasswordExpire = null;

            await _repo.UpdateAsync(user);
        }

        public async Task<bool> ChangePasswordAsync(string userId, ChangePasswordDto dto)
        {
            // 1. Kiểm tra xác nhận mật khẩu (Logic logic đơn giản nên check ngay đầu)
            if (dto.NewPassword != dto.ConfirmPassword)
                throw new Exception("Mật khẩu xác nhận không khớp.");

            // 2. Tìm user trong DB
            var user = await _repo.GetByIdAsync(userId);
            if (user == null) throw new Exception("Không tìm thấy người dùng.");

            // 3. QUAN TRỌNG: Kiểm tra mật khẩu CŨ có đúng không
            // Tận dụng hàm private VerifyPasswordHash có sẵn trong class này
            if (!VerifyPasswordHash(dto.CurrentPassword, user.PasswordHash, user.PasswordSalt))
            {
                throw new Exception("Mật khẩu hiện tại không chính xác.");
            }

            // 4. Tạo Hash và Salt cho mật khẩu MỚI
            // Tận dụng hàm private CreatePasswordHash có sẵn
            CreatePasswordHash(dto.NewPassword, out byte[] newHash, out byte[] newSalt);

            // 5. Cập nhật thông tin user
            user.PasswordHash = newHash;
            user.PasswordSalt = newSalt;

            // 6. Lưu xuống DB
            await _repo.UpdateAsync(user);

            return true;
        }
    }
}
