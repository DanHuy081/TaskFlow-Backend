using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model.DTOs
{
    public class CreateChecklistItemDto
    {
        public string ChecklistId { get; set; }
        public string Name { get; set; }
    }
}
