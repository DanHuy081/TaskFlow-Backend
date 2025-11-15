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
    public class ChecklistItemRepository : IChecklistItemRepository
    {
        private readonly ApplicationDbContext _context;

        public ChecklistItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ChecklistItemFL>> GetAllAsync()
        {
            return await _context.ChecklistItems
                .Include(i => i.User)
                .Include(i => i.Checklist)
                .ToListAsync();
        }

        public async Task<IEnumerable<ChecklistItemFL>> GetByChecklistIdAsync(string checklistId)
        {
            return await _context.ChecklistItems
                .Where(i => i.ChecklistId == checklistId)
                .Include(i => i.User)
                .ToListAsync();
        }

        public async Task<ChecklistItemFL> GetByIdAsync(string id)
        {
            return await _context.ChecklistItems
                .Include(i => i.User)
                .Include(i => i.Checklist)
                .FirstOrDefaultAsync(i => i.ChecklistItemId == id);
        }

        public async Task AddAsync(ChecklistItemFL item)
        {
            _context.ChecklistItems.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ChecklistItemFL item)
        {
            _context.ChecklistItems.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var item = await _context.ChecklistItems.FindAsync(id);
            if (item != null)
            {
                _context.ChecklistItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}
