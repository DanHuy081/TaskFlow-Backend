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
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;
        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<TaskFL>> GetAllAsync()
        {
            return await _context.Tasks.ToListAsync();
        }

        public async Task<TaskFL> GetByIdAsync(string id)
        {
            return await _context.Tasks.FindAsync(id);
        }

        public async Task AddAsync(TaskFL task)
        {
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TaskFL task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<TaskFL>> GetByListAsync(string listId)
        {
            return await _context.Tasks
                .Where(t => t.ListId == listId)
                .ToListAsync();
        }

        public async Task<List<TaskFL>> GetTasksByUserIdAsync(string userId)
        {
            // Giả sử bạn đã có _context hoặc _repo
            // Logic: Lấy task mà user này tạo hoặc được assign
            return await _context.Tasks
                .Include(t => t.List) // Include bảng List/Space nếu cần
                .Where(t => t.CreatorId == userId || t.TaskAssignees.Any(ta => ta.UserId == userId))
                .OrderByDescending(t => t.DateCreated)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskFL>> GetTasksDueInIntervalAsync(DateTime start, DateTime end)
        {
            // Ví dụ: Lấy các task có Deadline từ [Bây giờ] đến [1 tiếng nữa]
            // Và trạng thái chưa hoàn thành (giả sử Status != "Completed")
            return await _context.Tasks
                                 .Where(t => t.StartDate >= start &&
                                             t.DateClosed <= end &&
                                             t.Status != "Completed")
                                 .Include(t => t.TaskAssignees) // Nhớ Include bảng phân công để biết báo cho ai
                                 .ToListAsync();
        }
    }
}
