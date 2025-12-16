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
    public class CalendarRepository : ICalendarRepository
    {
        private readonly ApplicationDbContext _context;

        public CalendarRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TaskFL>> GetTasksForCalendarAsync(
            string userId,
            DateTime from,
            DateTime to)
        {
            return await _context.Tasks
                .Where(t =>
                    t.TaskAssignees.Any(a => a.UserId == userId) &&
                    t.DueDate >= from &&
                    t.DueDate <= to
                )
                .ToListAsync();
        }

        public async Task<TaskFL?> GetTaskByIdAsync(string taskId)
        {
            return await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);
        }

        public async Task UpdateTaskAsync(TaskFL task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }
    }

}
