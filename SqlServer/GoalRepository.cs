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
    public class GoalRepository : IGoalRepository
    {
        private readonly ApplicationDbContext _context;

        public GoalRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GoalFL>> GetAllAsync()
        {
            return await _context.Goals
                .Include(g => g.Teams)
                .ToListAsync();
        }

        public async Task<IEnumerable<GoalFL>> GetByTeamIdAsync(string teamId)
        {
            return await _context.Goals
                .Where(g => g.TeamId == teamId)
                .ToListAsync();
        }

        public async Task<GoalFL> GetByIdAsync(string id)
        {
            return await _context.Goals
                .Include(g => g.Teams)
                .FirstOrDefaultAsync(g => g.GoalId == id);
        }

        public async Task AddAsync(GoalFL goal)
        {
            _context.Goals.Add(goal);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(GoalFL goal)
        {
            _context.Goals.Update(goal);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var goal = await _context.Goals.FindAsync(id);
            if (goal != null)
            {
                _context.Goals.Remove(goal);
                await _context.SaveChangesAsync();
            }
        }
    }
}
