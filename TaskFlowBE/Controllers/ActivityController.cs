using LogicBusiness.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace TaskFlowBE.Controllers
{
    [Route("api/ActivityLogs")] // Hoặc api/teams
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityService _activityService;

        public ActivityController(IActivityService activityService)
        {
            _activityService = activityService;
        }

        // API: GET /api/activities/export/team-id-123
        [HttpGet("export/{teamId}")]
        public async Task<IActionResult> ExportHistory(Guid teamId)
        {
            try
            {
                // 1. Gọi Service để lấy file dưới dạng byte[]
                var fileContent = await _activityService.ExportTeamHistoryToExcelAsync(teamId);

                // 2. Đặt tên file
                string fileName = $"History_Team_{DateTime.Now:yyyyMMdd_HHmm}.xlsx";
                string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                // 3. Trả về file cho trình duyệt tải xuống
                return File(fileContent, contentType, fileName);
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                return StatusCode(500, "Lỗi khi xuất file: " + ex.Message);
            }
        }

        [HttpGet("team/{teamId}")]
        public async Task<IActionResult> GetTeamActivities(Guid teamId)
        {
            try
            {
                
                var logs = await _activityService.GetTeamHistoryAsync(teamId);

                // Trả về JSON (HTTP 200 OK)
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi Server: " + ex.Message);
            }
        }
    }
}
