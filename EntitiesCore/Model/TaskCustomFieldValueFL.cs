using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model
{
    [Table("TaskCustomFieldValues")]
    public class TaskCustomFieldValueFL
    {
        [Key]
        [Column(Order = 0)]
        public string TaskId { get; set; }

        [Key]
        [Column(Order = 1)]
        public string FieldId { get; set; }

        [Column("Value")]
        public string? Value { get; set; }

        [Column("DateUpdated")]
        public DateTime? DateUpdated { get; set; }

        // Navigation
        public TaskFL? Task { get; set; }
        public CustomFieldFL? CustomField { get; set; }
    }
}
