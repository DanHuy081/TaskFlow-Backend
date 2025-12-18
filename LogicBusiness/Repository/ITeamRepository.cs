using CoreEntities.Model;
using CoreEntities.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.Repository
{
    public interface ITeamRepository
    {
        Task<IEnumerable<Team>> GetAllAsync();
        Task<Team> GetByIdAsync(string id);
        Task AddAsync(Team team);
        Task UpdateAsync(Team team);
        Task DeleteTeamCascadeAsync(string teamId);
        //
        Task AddTeamAsync(Team team);
        Task AddTeamMemberAsync(TeamMember member);

        //

        // Kiểm tra user đã có trong team chưa
        Task<bool> IsMemberExistAsync(string teamId, string userId);

        // Thêm thành viên mới
        Task AddMemberAsync(TeamMember member);

        // Tìm user qua email (Hàm hỗ trợ)
        Task<UserFL?> GetUserByEmailAsync(string email);

        Task<List<TeamBriefDto>> GetTeamsByUserIdAsync(string userId);
    }
}
