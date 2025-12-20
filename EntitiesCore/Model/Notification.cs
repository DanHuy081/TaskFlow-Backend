using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model
{
    public class Notification
    {
        [Key]
        public Guid Id { get; set; } 

        [ForeignKey("User")]
        public Guid UserId { get; set; } // Người nhận thông báo (String để khớp với Identity)

        public string Title { get; set; } // Ví dụ: "Task mới"
        public string Message { get; set; } // Ví dụ: "Huy đã giao việc X cho bạn"
        //public string LinkAction { get; set; } // Ví dụ: "/tasks/task-id-123"

        public string Type { get; set; } = "Info"; // Info, Success, Warning, Error
        public bool IsRead { get; set; } = false; // Đã xem chưa
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
