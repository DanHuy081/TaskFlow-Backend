using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model.DTOs
{
    public class DashboardDto
    {
        // 1. Các con số thống kê (Cards)
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int InProgressTasks { get; set; }
        public int PendingTasks { get; set; }

        // 2. Tính phần trăm để vẽ thanh Progress Bar
        public double CompletionPercentage
        {
            get
            {
                if (TotalTasks == 0) return 0;
                return (double)CompletedTasks / TotalTasks * 100;
            }
        }

        // 3. Thống kê Workspace
        public int TotalTeams { get; set; }
        public int TotalSpaces { get; set; }

        // 4. Danh sách task gần đây
        public List<TaskSummaryDto> RecentTasks { get; set; }
    }

    // DTO nhỏ để hiển thị list task rút gọn
    public class TaskSummaryDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string DueDate { get; set; }
        public string Priority { get; set; }
    }
}
