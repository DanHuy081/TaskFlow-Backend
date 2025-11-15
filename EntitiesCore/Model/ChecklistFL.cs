using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model
{
    [Table("Checklists")]
    public class ChecklistFL
    {
        [Key]
        [Column("ChecklistId")]
        public string ChecklistId { get; set; }

        [Column("TaskId")]
        public string TaskId { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("DateCreated")]
        public DateTime? DateCreated { get; set; }

        // 🔗 Quan hệ tới Task
        [ForeignKey("TaskId")]
        public TaskFL Task { get; set; }

        // 🔗 1 Checklist có nhiều ChecklistItems
        public ICollection<ChecklistItemFL> ChecklistItems { get; set; }
    }
}
