using CoreEntities.Model;
using LogicBusiness.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlServer.Data;
using CoreEntities.Model.Enums;
using CoreEntities.Model.DTOs;

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

        public async Task<TeamRole?> GetUserRoleAsync(string userId, string teamId)
        {
            // Vì trong DB, TeamId là Guid, nên tham số truyền vào cũng phải là Guid
            // 2. Tìm thành viên trong DB
            var member = await _context.TeamMembers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == userId && x.TeamId == teamId);

            if (member == null) return null;

            // 3. XỬ LÝ LỖI "Cannot convert string to TeamRole":
            // Nếu trong DB, cột Role là string, ta phải Parse nó sang Enum
            if (Enum.TryParse<TeamRole>(member.Role.ToString(), out var roleEnum))
            {
                return roleEnum;
            }

            // Nếu parse thất bại (hoặc lưu sai), mặc định trả về Member
            return TeamRole.Member;
        }

        public async Task<TeamMember> GetMemberAsync(string teamId, string userId)
        {
            // Vì gộp bảng nên không cần .Include() nữa
            return await _context.TeamMembers
                .FirstOrDefaultAsync(m => m.TeamId == teamId && m.UserId == userId);
        }

        public async Task UpdateMemberAsync(TeamMember member)
        {
            _context.TeamMembers.Update(member);
            await _context.SaveChangesAsync();
        }
    }
}
