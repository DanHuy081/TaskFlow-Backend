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
    public class CustomFieldRepository : ICustomFieldRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomFieldRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CustomFieldFL>> GetAllAsync()
        {
            return await _context.CustomFields.ToListAsync();
        }

        public async Task<CustomFieldFL?> GetByIdAsync(string id)
        {
            return await _context.CustomFields.FindAsync(id);
        }

        public async Task<CustomFieldFL> CreateAsync(CustomFieldFL entity)
        {
            _context.CustomFields.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<CustomFieldFL> UpdateAsync(CustomFieldFL entity)
        {
            _context.CustomFields.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var existing = await _context.CustomFields.FindAsync(id);
            if (existing == null) return false;

            _context.CustomFields.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
