using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreEntities.Model
{
    [Table("Tasks")]
    public class TaskFL
    {
        [Key]
        [Column("Id")]
        public string Id { get; set; }

        [Column("ListId")]
        public string? ListId { get; set; }

        [Column("ParentTaskId")]
        public string? ParentTaskId { get; set; }

        [Column("Name")]
        public string? Name { get; set; }

        [Column("Description")]
        public string? Description { get; set; }

        [Column("Status")]
        public string? Status { get; set; }

        [Column("Priority")]
        public string? Priority { get; set; }

        [Column("DueDate")]
        public DateTime? DueDate { get; set; }

        [Column("StartDate")]
        public DateTime? StartDate { get; set; }

        [Column("TimeEstimate")]
        public long? TimeEstimate { get; set; }

        [Column("TimeSpent")]
        public long? TimeSpent { get; set; }

        [Column("CreatorId")]
        public string? CreatorId { get; set; }

        [Column("DateCreated")]
        public DateTime? DateCreated { get; set; }

        [Column("DateUpdated")]
        public DateTime? DateUpdated { get; set; }

        [Column("DateClosed")]
        public DateTime? DateClosed { get; set; }

        [Column("IsArchived")]
        public bool? IsArchived { get; set; }

        [Column("Url")]
        public string? Url { get; set; }
        
    }
}
