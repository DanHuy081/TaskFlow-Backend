using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model.Enums
{
    public enum TeamRole
    {
        Owner = 0, // Quyền lực nhất
        Admin = 1, // Quản lý
        Member = 2, // Nhân viên (Bị giới hạn)
        Viewer = 3  // Chỉ xem
    }
}
