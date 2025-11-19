using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model.DTOs
{
    public class TaskDto
    {
        public string Id { get; set; }
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
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public DateTime? DateClosed { get; set; }
        public bool? IsArchived { get; set; }
        public string? Url { get; set; }
    }
}
