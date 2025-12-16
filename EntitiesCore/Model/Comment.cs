using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model
{
    public class Comment
    {
        [Key]
        [Column("CommentId")]
        public string CommentId { get; set; }

        [Column("TaskId")]
        public string? TaskId { get; set; }

        [Column("UserId")]
        public string UserId { get; set; }

        [Column("CommentText")]
        public string CommentText { get; set; }

        [Column("IsEdited")]
        public bool? IsEdited { get; set; }

        [Column("DateCreated")]
        public DateTime? DateCreated { get; set; }

        [Column("DateUpdated")]
        public DateTime? DateUpdated { get; set; }
        [Column("VectorData")]
        public string? VectorData { get; set; }
        public TaskFL Task { get; set; }
       
    }
}
