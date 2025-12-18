using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model
{
    [Table("Conversations")]
    public class Conversation
    {
        [Key]
        public Guid ConversationId { get; set; } = Guid.NewGuid();

        [Required]
        public string UserId { get; set; } // FK tới bảng User của bạn

        public Guid? TeamId { get; set; } // Context team (Nullable)

        [MaxLength(200)]
        public string Title { get; set; } = "New Chat";

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public DateTime DateUpdated { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        // Navigation Properties
        public virtual ICollection<ChatMessage> Messages { get; set; }
    }
}
