using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model.DTOs
{
    public class ListDto
    {
        public string ListId { get; set; }
        public string? SpaceId { get; set; }
        public string? FolderId { get; set; }
        public string Name { get; set; }
        public string? Status { get; set; }
        public string? Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }

    public class ListEntity
    {
        public Guid ListId { get; set; }
        public Guid SpaceId { get; set; }
        public Space Space { get; set; }
    }

    public class Space
    {
        public Guid SpaceId { get; set; }
        public Guid TeamId { get; set; }
        public Team Team { get; set; }
    }

    public class Team
    {
        public Guid TeamId { get; set; }
        public string Name { get; set; }
    }

}
