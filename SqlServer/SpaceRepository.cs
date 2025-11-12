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
    public class SpaceRepository : ISpaceRepository
    {
        private readonly ApplicationDbContext _context;

        public SpaceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Space>> GetAllAsync()
        {
            return await _context.Spaces.ToListAsync();
        }

        public async Task<Space> GetByIdAsync(string id)
        {
            return await _context.Spaces.FindAsync(id);
        }

        public async Task AddAsync(Space space)
        {
            _context.Spaces.Add(space);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Space space)
        {
            _context.Spaces.Update(space);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var space = await _context.Spaces.FindAsync(id);
            if (space != null)
            {
                _context.Spaces.Remove(space);
                await _context.SaveChangesAsync();
            }
        }
    }
}
