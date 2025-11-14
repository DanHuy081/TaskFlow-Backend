using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace CoreEntities.Model
{
    [Table("TaskAssignees")]
    public class TaskAssignee
    {
        
        [Column("TaskId")]
        public string TaskId { get; set; }

       
        [Column("UserId")]
        public string UserId { get; set; }

        [Column("AssignedAt")]
        public DateTime? AssignedAt { get; set; }

        // 🔗 Quan hệ

        [ForeignKey("TaskId")]
        [JsonIgnore]
        public TaskFL Tasks { get; set; }

        [ForeignKey("UserId")]
        public UserFL UserFLs { get; set; }
    }
}
