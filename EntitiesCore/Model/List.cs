using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model
{
    [Table("Lists")]
    public class List
    {
        [Key]
        [Column("ListId")]
        public string ListId { get; set; }

        [Column("SpaceId")]
        public string SpaceId { get; set; }

        [Column("FolderId")]
        public string? FolderId { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("Status")]
        public string Status { get; set; }

        [Column("Priority")]
        public string Priority { get; set; }

        [Column("DueDate")]
        public DateTime? DueDate { get; set; }

        [Column("DateCreated")]
        public DateTime? DateCreated { get; set; }

        [Column("DateUpdated")]
        public DateTime? DateUpdated { get; set; }

        // Quan hệ 1 List có nhiều Task
        public ICollection<TaskFL> Tasks { get; set; } = new List<TaskFL>();
    }
}
