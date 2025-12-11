using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model.DTOs
{
    public class ChecklistItemDto
    {
        public string ChecklistItemId { get; set; }
        public string ChecklistId { get; set; }
        public string Name { get; set; }
        public bool IsResolved { get; set; }
        public int OrderIndex { get; set; }
        public DateTime DateCreated { get; set; }
        public string? ResolvedBy { get; set; }
        public DateTime? ResolvedAt { get; set; }
    }
}
