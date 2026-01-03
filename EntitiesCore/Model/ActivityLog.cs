using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model
{
    public class ActivityLog
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string UserId { get; set; } // Người thực hiện
        public Guid TeamId { get; set; }   // Hoạt động thuộc team nào

        public string Action { get; set; } // Ví dụ: "CREATE_TASK", "DELETE_MEMBER"
        public string EntityName { get; set; } // Ví dụ: "Task", "Project"
        public string EntityId { get; set; }   // ID của đối tượng bị tác động
        public string Description { get; set; } // Mô tả chi tiết: "Đã xóa task ABC"

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        // Navigation property (Optional)
        [ForeignKey("UserId")]
        public virtual UserFL User { get; set; }
    }
}
