using Microsoft.EntityFrameworkCore;
using CoreEntities.Model;
using CoreEntities.Model.DTOs;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using LogicBusiness.Repository;
using SqlServer.Data;

public class DashboardRepository : IDashboardRepository
{
    private readonly ApplicationDbContext _context;

    public DashboardRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardDto> GetDashboardStatisticsAsync(string userId, Guid? teamId)
    {
        var dto = new DashboardDto();

        // userId là string, DB cũng là string => So sánh trực tiếp được luôn!

        var query = _context.Tasks.AsQueryable();

        if (teamId.HasValue)
        {
            // Convert Guid to string for comparison
            var teamIdString = teamId.Value.ToString();

            // Lọc theo Team (Dùng ID truy vấn lồng nhau để an toàn nhất)
            var spaceIds = _context.Spaces
                .Where(s => s.TeamId == teamIdString)
                .Select(s => s.SpaceId);

            var listIds = _context.Lists
                .Where(l => spaceIds.Contains(l.SpaceId))
                .Select(l => l.ListId);

            query = query.Where(t => listIds.Contains(t.ListId));
        }

        else
        {
            // Dashboard cá nhân
            // Giờ đây câu lệnh này sẽ chạy ngon lành vì 2 bên đều là string
            query = query.Where(t => t.CreatorId == userId || t.TaskAssignees.Any(ta => ta.UserId == userId));
        }

        // --- Đếm số liệu ---
        dto.TotalTasks = await query.CountAsync();
        dto.CompletedTasks = await query.CountAsync(t => t.Status == "DONE" || t.Status == "COMPLETE");
        dto.InProgressTasks = await query.CountAsync(t => t.Status == "IN_PROGRESS" || t.Status == "DOING");
        dto.PendingTasks = await query.CountAsync(t => t.Status == "TODO" || t.Status == "PENDING");

        // --- Đếm Team và Space ---
        dto.TotalTeams = await _context.TeamMembers.CountAsync(tm => tm.UserId == userId);

        var userTeamIds = _context.TeamMembers
            .Where(tm => tm.UserId == userId)
            .Select(tm => tm.TeamId);

        dto.TotalSpaces = await _context.Spaces
            .Where(s => userTeamIds.Contains(s.TeamId))
            .CountAsync();

        // --- Lấy list task rút gọn ---
        dto.RecentTasks = await query
            .OrderByDescending(t => t.DateCreated)
            .Take(5)
            .Select(t => new TaskSummaryDto
            {
                Id = t.Id.ToString(),
                Name = t.Name,
                Status = t.Status,
                Priority = t.Priority,
                DueDate = t.DueDate.HasValue ? t.DueDate.Value.ToString("dd/MM/yyyy") : ""
            })
            .ToListAsync();

        return dto;
    }
}