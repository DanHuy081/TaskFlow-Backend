using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model.DTOs
{
    public class CommentCreateDto
    {
        public string TaskId { get; set; }
        public string UserId { get; set; }
        public string CommentText { get; set; }
    }
}
