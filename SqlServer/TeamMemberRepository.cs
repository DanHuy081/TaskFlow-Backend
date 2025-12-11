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
    public class TeamMemberRepository : ITeamMemberRepository
    {
        private readonly ApplicationDbContext _context;

        public TeamMemberRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TeamMember>> GetAllAsync()
        {
            return await _context.TeamMembers
                .Include(tm => tm.Teams)
                .Include(tm => tm.UserFLs)
                .ToListAsync();
        }

        public async Task<IEnumerable<TeamMember>> GetByTeamIdAsync(string teamId)
        {
            return await _context.TeamMembers
                .Where(tm => tm.TeamId == teamId)
                .Include(tm => tm.UserFLs)
                .ToListAsync();
        }

        public async Task<IEnumerable<TeamMember>> GetByUserIdAsync(string userId)
        {
            return await _context.TeamMembers
                .Where(tm => tm.UserId == userId)
                .Include(tm => tm.Teams)
                .ToListAsync();
        }

        public async Task<TeamMember> GetAsync(string teamId, string userId)
        {
            return await _context.TeamMembers
                .FirstOrDefaultAsync(tm => tm.TeamId == teamId && tm.UserId == userId);
        }

        public async Task AddAsync(TeamMember member)
        {
            _context.TeamMembers.Add(member);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TeamMember member)
        {
            _context.TeamMembers.Update(member);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string teamId, string userId)
        {
            var member = await GetAsync(teamId, userId);
            if (member != null)
            {
                _context.TeamMembers.Remove(member);
                await _context.SaveChangesAsync();
            }
        }
    }
}
