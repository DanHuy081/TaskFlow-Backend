using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model.DTOs
{
    public class PersonalSpaceDto
    {
        public string SpaceId { get; set; }
        public string SpaceName { get; set; }
        public string DefaultListId { get; set; } // Quan trọng: Để Frontend biết chỗ thêm task

        // Các chỉ số thống kê
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public double CompletionPercentage { get; set; }
    }
}
