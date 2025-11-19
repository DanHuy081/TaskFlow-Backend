using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model.DTOs
{
    public class TaskCreateDto
    {
        public string? ListId { get; set; }
        public string? ParentTaskId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public string? Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? StartDate { get; set; }
        public long? TimeEstimate { get; set; }
        public long? TimeSpent { get; set; }
        public string? CreatorId { get; set; }
        public string? Url { get; set; }
    }
}
