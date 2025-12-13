using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model.DTOs
{
    public class TaskAssigneeCreateDto
    {
        public string TaskId { get; set; }
        public string UserId { get; set; }
    }

}
