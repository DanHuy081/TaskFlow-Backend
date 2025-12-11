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
    public class TaskCustomFieldValueRepository : ITaskCustomFieldValueRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskCustomFieldValueRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskCustomFieldValueFL>> GetByTaskIdAsync(string taskId)
        {
            return await _context.TaskCustomFieldValues
                .Where(x => x.TaskId == taskId)
                .ToListAsync();
        }

        public async Task<TaskCustomFieldValueFL?> GetAsync(string taskId, string fieldId)
        {
            return await _context.TaskCustomFieldValues
                .FindAsync(taskId, fieldId);
        }

        public async Task<TaskCustomFieldValueFL> CreateAsync(TaskCustomFieldValueFL data)
        {
            _context.TaskCustomFieldValues.Add(data);
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<TaskCustomFieldValueFL> UpdateAsync(TaskCustomFieldValueFL data)
        {
            _context.TaskCustomFieldValues.Update(data);
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<bool> DeleteAsync(string taskId, string fieldId)
        {
            var record = await _context.TaskCustomFieldValues.FindAsync(taskId, fieldId);
            if (record == null)
                return false;

            _context.TaskCustomFieldValues.Remove(record);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
