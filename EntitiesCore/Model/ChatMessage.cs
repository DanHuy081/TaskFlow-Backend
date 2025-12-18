using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model
{
    [Table("Messages")]
    public class ChatMessage
    {
        [Key]
        public Guid MessageId { get; set; } = Guid.NewGuid();

        [Required]
        public Guid ConversationId { get; set; }

        [Required]
        public string Role { get; set; } // Lưu dạng string: "user", "assistant", "system"

        [Required]
        public string Content { get; set; } // Nội dung chat

        public string? Metadata { get; set; } // Lưu JSON context nếu cần

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("ConversationId")]
        public virtual Conversation Conversation { get; set; }
    }
}
