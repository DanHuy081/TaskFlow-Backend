using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model
{
    public class Comment
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("taskid")]
        public int? TaskId { get; set; }
        [Column("userid")]
        public int? UserId { get; set; }
        [Column("[content]")]
        public string Content { get; set; } = string.Empty;
        [Column("createdat")]
        public DateTime CreatedAt { get; set; }

        public Task? Task { get; set; }
        public User? User { get; set; }
    }
}
