using ClosedXML.Excel;
using CoreEntities.Model;
using LogicBusiness.Repository;
using LogicBusiness.UseCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.Service
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository _repo; // Gọi Repo, KHÔNG gọi DbContext

        public ActivityService(IActivityRepository repo)
        {
            _repo = repo;
        }

        public async Task LogAsync(string userId, Guid teamId, string action, string entityName, string entityId, string description)
        {
            var log = new ActivityLog
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                TeamId = teamId,
                Action = action,
                EntityName = entityName,
                EntityId = entityId,
                Description = description,
                DateCreated = DateTime.UtcNow
            };

            await _repo.AddLogAsync(log);
        }

        public async Task<byte[]> ExportTeamHistoryToExcelAsync(Guid teamId)
        {
            // 1. Lấy dữ liệu từ Repo
            var logs = await _repo.GetLogsByTeamIdAsync(teamId);

            // 2. Xử lý Logic tạo file Excel (Business Logic)
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("NhatKyHoatDong");

                // Tạo Header
                worksheet.Cell(1, 1).Value = "STT";
                worksheet.Cell(1, 2).Value = "Thời gian";
                worksheet.Cell(1, 3).Value = "Người thực hiện";
                worksheet.Cell(1, 4).Value = "Hành động";
                worksheet.Cell(1, 5).Value = "Chi tiết";
                worksheet.Cell(1, 6).Value = "Đối tượng";

                // Style Header
                var header = worksheet.Range("A1:F1");
                header.Style.Font.Bold = true;
                header.Style.Fill.BackgroundColor = XLColor.CornflowerBlue;
                header.Style.Font.FontColor = XLColor.White;

                // Đổ dữ liệu
                int row = 2;
                int stt = 1;
                foreach (var log in logs)
                {
                    worksheet.Cell(row, 1).Value = stt++;
                    worksheet.Cell(row, 2).Value = log.DateCreated.ToLocalTime().ToString("dd/MM/yyyy HH:mm");
                    worksheet.Cell(row, 3).Value = log.User?.FullName ?? log.User?.Username ?? "Unknown";
                    worksheet.Cell(row, 4).Value = log.Action;
                    worksheet.Cell(row, 5).Value = log.Description;
                    worksheet.Cell(row, 6).Value = $"{log.EntityName} ({log.EntityId})";
                    row++;
                }

                worksheet.Columns().AdjustToContents();

                // Lưu vào MemoryStream và trả về byte[]
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }

        public async Task<List<ActivityLog>> GetTeamHistoryAsync(Guid teamId)
        {
            // Gọi xuống Repository để lấy dữ liệu
            return await _repo.GetLogsByTeamIdAsync(teamId);
        }
    }
}
