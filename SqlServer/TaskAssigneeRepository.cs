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

        public async Task<List<TaskAssignee>> GetByTaskIdAsync(string taskId)
        {
            return await _context.TaskAssignees
                .Where(x => x.TaskId == taskId)
                .Include(x => x.UserFLs)
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

        public async Task<TaskAssignee> AddAsync(TaskAssignee entity)
        {
            await _context.TaskAssignees.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(string taskId, string userId)
        {
            var assignee = await _context.TaskAssignees
                .FirstOrDefaultAsync(x => x.TaskId == taskId && x.UserId == userId);

            if (assignee == null) return false;

            _context.TaskAssignees.Remove(assignee);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
