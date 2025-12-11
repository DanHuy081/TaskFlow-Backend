using CoreEntities.Model;
using LogicBusiness.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlServer.Data;

namespace SqlServer
{
    public class TaskAssigneeRepository : ITaskAssigneeRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskAssigneeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskAssignee>> GetAllAsync()
        {
            return await _context.TaskAssignees
                .Include(a => a.UserFLs)
                .Include(a => a.Tasks)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskAssignee>> GetByTaskIdAsync(string taskId)
        {
            return await _context.TaskAssignees
                .Where(a => a.TaskId == taskId)
                .Include(a => a.UserFLs)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskAssignee>> GetByUserIdAsync(string userId)
        {
            return await _context.TaskAssignees
                .Where(a => a.UserId == userId)
                .Include(a => a.Tasks)
                .ToListAsync();
        }

        public async Task<TaskAssignee> GetAsync(string taskId, string userId)
        {
            return await _context.TaskAssignees
                .FirstOrDefaultAsync(a => a.TaskId == taskId && a.UserId == userId);
        }

        public async Task AddAsync(TaskAssignee assignee)
        {
            _context.TaskAssignees.Add(assignee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string taskId, string userId)
        {
            var record = await GetAsync(taskId, userId);
            if (record != null)
            {
                _context.TaskAssignees.Remove(record);
                await _context.SaveChangesAsync();
            }
        }
    }
}
