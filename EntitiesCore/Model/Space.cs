using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model
{
    [Table("Spaces")]
    public class Space
    {
        [Key]
        [Column("SpaceId")]
        public string SpaceId { get; set; }

        [Column("TeamId")]
        public string? TeamId { get; set; }

        [Column("Name")]
        public string? Name { get; set; }

        [Column("Color")]
        public string? Color { get; set; }

        [Column("IsPrivate")]
        public bool? IsPrivate { get; set; }

        [Column("Settings")]
        public string? Settings { get; set; }

        [Column("DateCreated")]
        public DateTime? DateCreated { get; set; }

        [Column("DateUpdated")]
        public DateTime? DateUpdated { get; set; }

        // Một Space có nhiều Folder
        public ICollection<Folder> Folders { get; set; } = new List<Folder>();
        public virtual Team Teams { get; set; }
    }
}
