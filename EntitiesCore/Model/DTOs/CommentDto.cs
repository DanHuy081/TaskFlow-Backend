using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model.DTOs
{
    public class CommentDto
    {
        public string CommentId { get; set; }
        public string? TaskId { get; set; }
        public string UserId { get; set; }
        public string CommentText { get; set; }
        public bool? IsEdited { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}
