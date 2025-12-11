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
    public class TimeEntryRepository : ITimeEntryRepository
    {
        private readonly ApplicationDbContext _context;

        public TimeEntryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TimeEntryFL>> GetAllAsync()
        {
            return await _context.TimeEntries.ToListAsync();
        }

        public async Task<TimeEntryFL?> GetByIdAsync(string id)
        {
            return await _context.TimeEntries.FindAsync(id);
        }

        public async Task<TimeEntryFL> CreateAsync(TimeEntryFL entry)
        {
            _context.TimeEntries.Add(entry);
            await _context.SaveChangesAsync();
            return entry;
        }

        public async Task<TimeEntryFL> UpdateAsync(TimeEntryFL entry)
        {
            _context.TimeEntries.Update(entry);
            await _context.SaveChangesAsync();
            return entry;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var existing = await _context.TimeEntries.FindAsync(id);
            if (existing == null) return false;

            _context.TimeEntries.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
