using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model
{
    public class ConversationSummary
    {
        [Key]
        public Guid ConversationId { get; set; }
        public string Summary { get; set; } = string.Empty;
        public DateTime DateUpdated { get; set; } = DateTime.UtcNow;
    }
}
