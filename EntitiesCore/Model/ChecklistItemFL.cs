using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model
{
    [Table("ChecklistItems")]
    public class ChecklistItemFL
    {
        [Key]
        [Column("ChecklistItemId")]
        public string ChecklistItemId { get; set; }

        [Column("ChecklistId")]
        public string ChecklistId { get; set; }

        [Column("Name")]
        public string? Name { get; set; }

        [Column("IsResolved")]
        public bool? IsResolved { get; set; }

        [Column("OrderIndex")]
        public int? OrderIndex { get; set; }

        [Column("DateCreated")]
        public DateTime? DateCreated { get; set; }

        [Column("ResolvedBy")]
        public string? ResolvedBy { get; set; }

        [Column("ResolvedAt")]
        public DateTime? ResolvedAt { get; set; }

        // 🔗 Quan hệ
        [ForeignKey("ChecklistId")]
        public ChecklistFL Checklist { get; set; }

        [ForeignKey("ResolvedBy")]
        public UserFL User { get; set; }
    }
}
