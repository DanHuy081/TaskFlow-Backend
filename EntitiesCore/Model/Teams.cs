using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model
{
    [Table("Teams")]
    public class Team
    {
        [Key]
        [Column("TeamId")]
        public string TeamId { get; set; }

        [Column("Name")]
        public string? Name { get; set; }

        [Column("Description")]
        public string? Description { get; set; }

        [Column("Color")]
        public string? Color { get; set; }

        [Column("Avatar")]
        public string? Avatar { get; set; }

        [Column("DateCreated")]
        public DateTime? DateCreated { get; set; }

        [Column("DateUpdated")]
        public DateTime? DateUpdated { get; set; }

        // 🔗 Quan hệ 1 Team có nhiều TeamMembers
        public ICollection<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();
        public ICollection<GoalFL> Goals { get; set; }
    }
}
