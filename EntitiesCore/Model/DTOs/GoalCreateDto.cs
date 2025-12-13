using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model.DTOs
{
    public class GoalCreateDto
    {
        public string TeamId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
    }

}
