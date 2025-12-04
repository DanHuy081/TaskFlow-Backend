using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model.DTOs
{
    public class SpaceDto
    {
        public string SpaceId { get; set; }
        public string TeamId { get; set; }
        public string Name { get; set; }
        public string? Color { get; set; }
        public bool? IsPrivate { get; set; }
        public string? Settings { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}
