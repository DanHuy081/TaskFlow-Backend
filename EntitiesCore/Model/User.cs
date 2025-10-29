using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model
{
    public class User
    {
        public int UserId { get; set; }              // Khóa chính
        public string? FullName { get; set; }     // Tên đăng nhập
        public string? Email { get; set; }        // Email
        public string? PasswordHash { get; set; } // Mật khẩu đã mã hóa
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
    }

    public class UserRegisterDto
    {
        [Required(ErrorMessage = "Username is required")]
        [MinLength(3)]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6)]
        public string? Password { get; set; }
    }
}
