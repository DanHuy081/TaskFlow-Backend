using CoreEntities.Model;
using LogicBusiness.Repository;
using Microsoft.EntityFrameworkCore;
using SqlServer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlServer
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly ApplicationDbContext _context;

        public ActivityRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddLogAsync(ActivityLog log)
        {
            _context.activityLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ActivityLog>> GetLogsByTeamIdAsync(Guid teamId)
        {
            return await _context.activityLogs
                .Include(x => x.User) // Join để lấy tên người làm
                .Where(x => x.TeamId == teamId)
                .OrderByDescending(x => x.DateCreated) // Mới nhất lên đầu
                .ToListAsync();
        }
    }
}
