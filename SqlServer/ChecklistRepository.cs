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
    public class ChecklistRepository : IChecklistRepository
    {
        private readonly ApplicationDbContext _context;

        public ChecklistRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ChecklistFL>> GetAllAsync()
        {
            return await _context.Checklists
                .Include(c => c.ChecklistItems)
                .ToListAsync();
        }

        public async Task<IEnumerable<ChecklistFL>> GetByTaskIdAsync(string taskId)
        {
            return await _context.Checklists
                .Where(c => c.TaskId == taskId)
                .Include(c => c.ChecklistItems)
                .ToListAsync();
        }

        public async Task<ChecklistFL> GetByIdAsync(string id)
        {
            return await _context.Checklists
                .Include(c => c.ChecklistItems)
                .FirstOrDefaultAsync(c => c.ChecklistId == id);
        }

        public async Task<ChecklistFL> CreateAsync(ChecklistFL checklist)
        {
            _context.Checklists.Add(checklist);
            await _context.SaveChangesAsync();
            return checklist;
        }

        public async Task UpdateAsync(ChecklistFL checklist)
        {
            _context.Checklists.Update(checklist);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var checklist = await _context.Checklists.FindAsync(id);
            if (checklist != null)
            {
                _context.Checklists.Remove(checklist);
                await _context.SaveChangesAsync();
            }
        }
    }
}
