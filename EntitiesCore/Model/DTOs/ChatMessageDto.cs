using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model.DTOs
{
    public class SendMessageDto
    {
        public Guid ChatRoomId { get; set; }
        public string Content { get; set; }
    }

    public class ChatMessageDto
    {
        public Guid MessageId { get; set; }
        public string Content { get; set; }
        public string SenderName { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
