using CoreEntities.Model;
using LogicBusiness.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlServer.Data;

namespace SqlServer.Mapping
{
    public class TaskTagRepository : ITaskTagRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskTagRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskTag>> GetTagsByTaskAsync(string taskId)
        {
            return await _context.TaskTags.Where(t => t.TaskId == taskId).ToListAsync();
        }

        public async Task AddTagToTaskAsync(TaskTag taskTag)
        {
            _context.TaskTags.Add(taskTag);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveTagFromTaskAsync(string taskId, string tagId)
        {
            var record = await _context.TaskTags
                .FirstOrDefaultAsync(t => t.TaskId == taskId && t.TagId == tagId);
            if (record != null)
            {
                _context.TaskTags.Remove(record);
                await _context.SaveChangesAsync();
            }
        }
    }
}
