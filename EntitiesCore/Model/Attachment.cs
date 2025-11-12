using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model
{
    [Table("Attachments")]
    public class Attachment
    {
        [Key]
        [Column("AttachmentId")]
        public string? AttachmentId { get; set; }

        [Column("TaskId")]
        public string? TaskId { get; set; }

        [Column("CommentId")]
        public string? CommentId { get; set; }

        [Column("FileName")]
        public string? FileName { get; set; }

        [Column("Url")]
        public string? Url { get; set; }

        [Column("SizeBytes")]
        public long? SizeBytes { get; set; }

        [Column("MimeType")]
        public string? MimeType { get; set; }

        [Column("UploadedBy")]
        public string? UploadedBy { get; set; }

        [Column("DateAdded")]
        public DateTime? DateAdded { get; set; }

        // 🔗 Navigation properties
        [ForeignKey("TaskId")]
        public TaskFL Task { get; set; }

        [ForeignKey("CommentId")]
        public Comment Comment { get; set; }
    }
}
