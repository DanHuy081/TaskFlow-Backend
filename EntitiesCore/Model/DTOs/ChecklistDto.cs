using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model.DTOs
{
    public class ChecklistDto
    {
        public string ChecklistId { get; set; }
        public string TaskId { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
