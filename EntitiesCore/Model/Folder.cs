using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model
{
    [Table("Folders")]
    public class Folder
    {
        [Key]
        [Column("FolderId")]
        public string FolderId { get; set; }

        [Column("SpaceId")]
        public string SpaceId { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("Hidden")]
        public bool? Hidden { get; set; }

        [Column("Archived")]
        public bool? Archived { get; set; }

        [Column("DateCreated")]
        public DateTime? DateCreated { get; set; }

        [Column("DateUpdated")]
        public DateTime? DateUpdated { get; set; }

        // 1 Folder có nhiều List
        public ICollection<List> Lists { get; set; } = new List<List>();
    }
}
