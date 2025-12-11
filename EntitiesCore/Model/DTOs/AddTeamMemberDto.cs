using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model.DTOs
{
    public class AddTeamMemberDto
    {
        public string Email { get; set; } // Người dùng nhập email người muốn mời
        public string Role { get; set; } = "Member"; // Mặc định là Member
    }

    public class AddMemberDto
    {
        public string Email { get; set; }
    }
}
