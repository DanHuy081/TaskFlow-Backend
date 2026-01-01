using CoreEntities.Model;
using LogicBusiness.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlServer.Data;
using CoreEntities.Model.DTOs;
using Team = CoreEntities.Model.Team;

namespace SqlServer
{
    public class TeamRepository : ITeamRepository
    {
        private readonly ApplicationDbContext _context;

        public TeamRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // --- CÁC HÀM CŨ GIỮ NGUYÊN ---
        public async Task<IEnumerable<Team>> GetAllAsync() => await _context.Teams.ToListAsync();
        public async Task<Team> GetByIdAsync(string id) => await _context.Teams.FindAsync(id);
        public async Task AddAsync(Team team) { _context.Teams.Add(team); await _context.SaveChangesAsync(); }
        public async Task UpdateAsync(Team team) { _context.Teams.Update(team); await _context.SaveChangesAsync(); }
        public async Task AddTeamAsync(Team team) { _context.Teams.Add(team); await _context.SaveChangesAsync(); }
        public async Task AddTeamMemberAsync(TeamMember member) { _context.TeamMembers.Add(member); await _context.SaveChangesAsync(); }
        public async Task<bool> IsMemberExistAsync(string teamId, string userId) => await _context.TeamMembers.AnyAsync(tm => tm.TeamId == teamId && tm.UserId == userId);
        public async Task AddMemberAsync(TeamMember member) { await _context.TeamMembers.AddAsync(member); await _context.SaveChangesAsync(); }
        public async Task<UserFL?> GetUserByEmailAsync(string email) => await _context.UserFLs.FirstOrDefaultAsync(u => u.Email == email);

        public async Task DeleteTeamCascadeAsync(string teamId)
        {
            // (Giữ nguyên logic xóa cascade của bạn)
            var spaces = await _context.Spaces.Where(s => s.TeamId == teamId).ToListAsync();
            foreach (var space in spaces) { _context.Spaces.Remove(space); }

            var goals = await _context.Goals.Where(g => g.TeamId == teamId).ToListAsync();
            _context.Goals.RemoveRange(goals);

            var members = await _context.TeamMembers.Where(tm => tm.TeamId == teamId).ToListAsync();
            _context.TeamMembers.RemoveRange(members);

            var team = await _context.Teams.FindAsync(teamId);
            if (team != null) _context.Teams.Remove(team);

            await _context.SaveChangesAsync();
        }

        public async Task<List<TeamBriefDto>> GetTeamsByUserIdAsync(string userId)
        {
            var data = await (
                from tm in _context.Set<TeamMember>()
                join t in _context.Set<CoreEntities.Model.Team>() on tm.TeamId equals t.TeamId
                where tm.UserId == userId
                select new TeamBriefDto
                {
                    TeamId = t.TeamId.ToString(),
                    Name = t.Name
                }
            ).Distinct().ToListAsync();

            return data;
        }

        // --- ✅ HÀM MỚI QUAN TRỌNG CHO CHATBOT ---
        // Hàm này lấy Team kèm theo danh sách Thành viên chi tiết
        public async Task<List<Team>> GetTeamsWithMembersByUserIdAsync(string userId)
        {
            return await _context.Teams
                .Include(t => t.TeamMembers)
                    .ThenInclude(tm => tm.UserFLs) // Load thông tin User (Tên, Email...)
                .Where(t => t.TeamMembers.Any(tm => tm.UserId == userId))
                .ToListAsync();
        }
    }
}