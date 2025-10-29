using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model.DTOs
{
    public class CreateTaskDto
    {
        [Required]
        public int ProjectId { get; set; }

        [Required]
        [StringLength(255)]
        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? AssignedTo { get; set; }

        public string? CreateBy { get; set; } // API có thể tự gán giá trị này nếu có xác thực

        [Required]
        public string? Status { get; set; } // Nên có giá trị mặc định

        public string? Priority { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime DueDate { get; set; }
    }
}
