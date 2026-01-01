using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model.DTOs
{
    public class ChatRequestDto
    {
        public Guid? ConversationId { get; set; } // Nếu null = Tạo cuộc hội thoại mới
        public string Message { get; set; }
        public Guid? CurrentTeamId { get; set; } // Context team hiện tại
        public Guid? CurrentSpaceId { get; set; }

        public Guid? CurrentListId { get; set; }
    }

    public class ChatResponseDto
    {
        public Guid ConversationId { get; set; }
        public string Title { get; set; }
        public string Reply { get; set; } // Câu trả lời của AI
        // Có thể thêm List<AIActionDto> Actions nếu cần sau này
    }
}
