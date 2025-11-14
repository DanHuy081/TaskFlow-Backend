using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model
{
    [Table("TeamMembers")]
    public class TeamMember
    {
        
        [Column("TeamId")]
        public string TeamId { get; set; }

        
        [Column("UserId")]
        public string UserId { get; set; }

        [Column("Role")]
        public string Role { get; set; }

        [Column("DateJoined")]
        public DateTime? DateJoined { get; set; }

        // 🔗 Quan hệ
        [ForeignKey("TeamId")]
        public Team Teams { get; set; }

        [ForeignKey("UserId")]
        public User Users { get; set; }
    }
}
