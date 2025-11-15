using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model
{
    [Table("TimeEntries")]
    public class TimeEntryFL
    {
        [Key]
        [Column("TimeEntryId")]
        public string TimeEntryId { get; set; }

        [Column("TaskId")]
        public string? TaskId { get; set; }

        [Column("UserId")]
        public string? UserId { get; set; }

        [Column("DurationMilliseconds")]
        public long? DurationMilliseconds { get; set; }

        [Column("StartTime")]
        public DateTime? StartTime { get; set; }

        [Column("EndTime")]
        public DateTime? EndTime { get; set; }

        [Column("Description")]
        public string? Description { get; set; }

        [Column("DateCreated")]
        public DateTime? DateCreated { get; set; }

        // Navigation
        public TaskFL? Task { get; set; }
        public UserFL? User { get; set; }
    }
}
