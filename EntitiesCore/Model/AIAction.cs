using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model
{
    [Table("AIActions")]
    public class AIAction
    {
        [Key]
        public Guid ActionId { get; set; } = Guid.NewGuid();

        public Guid MessageId { get; set; } // Link tới tin nhắn kích hoạt action này

        [Required]
        public string ActionType { get; set; } // "CreateTask", "UpdateTask",...

        public string EntityType { get; set; } // "Task", "Space",...

        public Guid? EntityId { get; set; } // ID của đối tượng bị tác động

        public string? Parameters { get; set; } // JSON input params

        public string? Result { get; set; } // JSON kết quả trả về

        public DateTime DateExecuted { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("MessageId")]
        public virtual ChatMessage Message { get; set; }
    }
}
