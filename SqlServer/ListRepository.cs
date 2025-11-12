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
    public class ListRepository : IListRepository
    {
        private readonly ApplicationDbContext _context;

        public ListRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ListFL>> GetAllAsync()
        {
            return await _context.Lists.ToListAsync();
        }

        public async Task<ListFL> GetByIdAsync(string id)
        {
            return await _context.Lists.FindAsync(id);
        }

        public async Task AddAsync(ListFL list)
        {
            _context.Lists.Add(list);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ListFL list)
        {
            _context.Lists.Update(list);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var list = await _context.Lists.FindAsync(id);
            if (list != null)
            {
                _context.Lists.Remove(list);
                await _context.SaveChangesAsync();
            }
        }
    }
}
