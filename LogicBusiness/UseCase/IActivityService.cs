using CoreEntities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.UseCase
{
    public interface IActivityService
    {
        // Hàm ghi log (để các Service khác gọi)
        Task LogAsync(string userId, Guid teamId, string action, string entityName, string entityId, string description);

        // Hàm xuất Excel (Trả về mảng byte của file)
        Task<byte[]> ExportTeamHistoryToExcelAsync(Guid teamId);

        Task<List<ActivityLog>> GetTeamHistoryAsync(Guid teamId);
    }
}
