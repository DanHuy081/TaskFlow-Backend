using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model
{
    [Table("Goals")]
    public class GoalFL
    {
        [Key]
        [Column("GoalId")]
        public string GoalId { get; set; }

        [Column("TeamId")]
        public string TeamId { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("Description")]
        public string? Description { get; set; }

        [Column("DueDate")]
        public DateTime? DueDate { get; set; }

        [Column("Progress")]
        public double? Progress { get; set; }

        [Column("DateCreated")]
        public DateTime? DateCreated { get; set; }
        [Column("VectorData")]
        public string? VectorData { get; set; }
        // 🔗 Quan hệ
        [ForeignKey("TeamId")]
        public Team Teams { get; set; }
    }
}
