using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model.DTOs
{
    public class TeamBriefDto
    {
        public string TeamId { get; set; } = "";
        public string Name { get; set; } = "";
        public object Description { get; set; }
    }
}
