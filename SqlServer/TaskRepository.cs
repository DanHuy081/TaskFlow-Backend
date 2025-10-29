using CoreEntities.Model;
using LogicBusiness.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlowBE.Data;

namespace SqlServer
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;
        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<TaskFL>> GetAllTasksAsync()
        {
            // Lấy tất cả Task từ CSDL
            // AsNoTracking() giúp tăng hiệu suất cho các truy vấn chỉ đọc
            return await _context.Tasks.AsNoTracking().ToListAsync();
        }

        public async Task<TaskFL> GetByIdAsync(int id)
        {
            return await _context.Tasks.FindAsync(id);
        }


        public async Task<TaskFL> CreateTaskAsync(TaskFL task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<TaskFL> UpdateAsync(TaskFL task)
        {
            var existing = await _context.Tasks.FindAsync(task.Id);
            if (existing == null) return null;

            _context.Entry(existing).CurrentValues.SetValues(task);
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
