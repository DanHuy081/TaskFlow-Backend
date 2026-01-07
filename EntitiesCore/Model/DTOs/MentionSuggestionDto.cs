using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model.DTOs
{
    public class MentionSuggestionDto
    {
        public string Id { get; set; }          // ID của User hoặc Task
        public string DisplayText { get; set; } // Tên hiển thị
        public string Type { get; set; }        // "User" hoặc "Task"
        public string? Avatar { get; set; }     // (Optional) Avatar user
    }
}
