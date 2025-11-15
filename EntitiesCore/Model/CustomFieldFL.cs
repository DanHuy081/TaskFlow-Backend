using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model
{
    [Table("CustomFields")]
    public class CustomFieldFL
    {
        [Key]
        [Column("FieldId")]
        public string FieldId { get; set; }

        [Column("Name")]
        public string? Name { get; set; }

        [Column("FieldType")]
        public string? FieldType { get; set; }

        // JSON string chứa các option (dropdown, select…)
        [Column("Options")]
        public string? Options { get; set; }

        [Column("DateCreated")]
        public DateTime? DateCreated { get; set; }

        public ICollection<TaskCustomFieldValueFL> Values { get; set; }
    }
}
