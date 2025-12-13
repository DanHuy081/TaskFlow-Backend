using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model.DTOs
{
    public class TaskAssigneeDto
    {
        public string TaskId { get; set; }
        public string UserId { get; set; }
        public DateTime AssignedAt { get; set; }

        // Gọn: chỉ trả thông tin user cần thiết
        public UserMiniDto User { get; set; }
    }

}
