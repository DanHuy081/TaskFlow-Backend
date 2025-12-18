using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model
{
    public class KnowledgeChunk
    {
        public Guid KnowledgeChunkId { get; set; }
        public string Title { get; set; } = string.Empty;     // VD: "DB Schema - Tasks"
        public string Tags { get; set; } = string.Empty;      // VD: "db,tasks,schema"
        public string Content { get; set; } = string.Empty;   // Nội dung chunk
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime DateUpdated { get; set; } = DateTime.UtcNow;
    }
}
