using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SqlServer.Data;
using CoreEntities.Model;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using CoreEntities.Model.DTOs;
using LogicBusiness.UseCase;
using Microsoft.AspNetCore.Authorization;
using LogicBusiness.Repository;
using LogicBusiness.Service;

namespace TaskFlowBE.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        private readonly IUserRepository _userRepo;

        public AuthController(IAuthService auth, IUserRepository userRepo)
        {
            _auth = auth;
            _userRepo = userRepo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            await _auth.RegisterAsync(dto);
            return Ok(new { message = "User registered successfully" });
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var token = await _auth.LoginAsync(dto);
            if (token == null) return Unauthorized();
            return Ok(new { token });
        }

        [HttpGet("me")]
        [Authorize] // Bắt buộc phải có Token mới gọi được
        public async Task<IActionResult> GetMe()
        {
            try
            {
                // 1. Lấy User ID từ trong Token (ClaimTypes.NameIdentifier)
                // Token đã được giải mã tự động bởi Middleware và lưu vào biến 'User'
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("Token không hợp lệ hoặc thiếu thông tin.");
                }

                // 2. Truy vấn Database để lấy thông tin mới nhất (bao gồm Role)
                // (Lý do nên query DB: để lỡ Admin vừa đổi quyền thì lấy được ngay, 
                // không phụ thuộc vào thông tin cũ trong Token)
                var user = await _userRepo.GetByIdAsync(userId);
                // Lưu ý: Bạn cần đảm bảo Repo có hàm GetByIdAsync hoặc tương tự

                if (user == null)
                {
                    return NotFound("Không tìm thấy người dùng.");
                }

                // 3. Trả về thông tin cho Frontend (ẩn password hash đi)
                return Ok(new
                {
                    user.UserId,
                    user.Username,
                    user.Email,
                    user.Role, // <--- CÁI FE ĐANG CẦN LÀ ĐÂY
                    user.DateCreated
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(
        ForgotPasswordRequestDto dto)
        {
            await _auth.ForgotPasswordAsync(dto.Email);
            return Ok("Nếu email tồn tại, link reset đã được gửi");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(
            ResetPasswordRequestDto dto)
        {
            await _auth.ResetPasswordAsync(
                dto.Token,
                dto.NewPassword);

            return Ok("Đổi mật khẩu thành công");
        }

        [HttpPost("change-password")]
        [Authorize] // Bắt buộc phải có Token (đã đăng nhập)
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            try
            {
                // Lấy UserId từ Token (ClaimTypes.NameIdentifier)
                // User.FindFirst(...) là cách lấy thông tin từ token người đang đăng nhập
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("Không xác định được người dùng.");

                await _auth.ChangePasswordAsync(userId, dto);

                return Ok(new { message = "Đổi mật khẩu thành công!" });
            }
            catch (Exception ex)
            {
                // Trả về lỗi để Frontend hiển thị (ví dụ: Sai mật khẩu cũ)
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}