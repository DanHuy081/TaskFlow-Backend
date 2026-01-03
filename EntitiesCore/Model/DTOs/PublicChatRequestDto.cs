using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model.DTOs
{
    public class PublicChatRequestDto
    {
        public string Message { get; set; }
        public string SessionId { get; set; } // Thay thế cho UserId
    }
}
