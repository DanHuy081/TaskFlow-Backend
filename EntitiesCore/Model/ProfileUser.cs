using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model
{
    [Table("Users")]
    public class UserFL
    {
        [Key]
        [Column("UserId")]
        public string UserId { get; set; }

        [Column("Username")]
        public string Username { get; set; }

        [Column("Email")]
        public string Email { get; set; }

        [Column("FullName")]
        public string FullName { get; set; }

        [Column("Color")]
        public string Color { get; set; }

        [Column("ProfilePicture")]
        public string? ProfilePicture { get; set; }

        [Column("Role")]
        public string Role { get; set; }

        [Column("Timezone")]
        public string? Timezone { get; set; }

        [Column("DateCreated")]
        public DateTime? DateCreated { get; set; }

        [Column("DateUpdated")]
        public DateTime? DateUpdated { get; set; }

        // Quan hệ với TeamMembers (User → TeamMembers)
        public ICollection<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();
    }
}
