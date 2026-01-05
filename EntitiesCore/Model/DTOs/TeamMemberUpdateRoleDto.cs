using CoreEntities.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model.DTOs
{
    public class TeamMemberUpdateRoleDto
    {
        public string UserId { get; set; }
        public string TeamId { get; set; }
        public string? Role { get; set; }

        public bool CanCreateTasks { get; set; } = false;
        public bool CanEditTasks { get; set; } = false;
        public bool CanDeleteTasks { get; set; } = false;
        public bool CanSetTaskDueDate { get; set; } = false; // Sửa ngày tháng
        public bool CanChangeTaskPriority { get; set; } = false; // Đổi priority
        public bool CanChangeTaskStatus { get; set; } = true;    // Đổi status (kéo thả) - Mặc định thường là true
        public bool CanAssignTasks { get; set; } = false;
        public bool CanCommentOnTasks { get; set; } = true;      // Bình luận
        public bool CanUploadFiles { get; set; } = true;         // Upload files
    }
}
