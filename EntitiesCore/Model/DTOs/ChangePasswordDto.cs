using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model.DTOs
{
    public class ChangePasswordDto
    {
        public string CurrentPassword { get; set; } // Mật khẩu hiện tại
        public string NewPassword { get; set; }     // Mật khẩu mới
        public string ConfirmPassword { get; set; } // Xác nhận mật khẩu mới
    }
}
