using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model.DTOs
{
    public class CalendarTaskDto
    {
        public string Id { get; set; }
        public string Title { get; set; }

        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }

        public string Status { get; set; }
    }

}
