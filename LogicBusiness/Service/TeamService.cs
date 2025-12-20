using CoreEntities.Model;
using CoreEntities.Model.DTOs;
using LogicBusiness.Repository;
using LogicBusiness.UseCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Team = CoreEntities.Model.Team;

namespace LogicBusiness.Service
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _repo;

        public TeamService(ITeamRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Team>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Team> GetByIdAsync(string id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task AddAsync(Team team)
        {
            team.TeamId = Guid.NewGuid().ToString();
            team.DateCreated = DateTime.UtcNow;
            await _repo.AddAsync(team);
        }

        public async Task UpdateAsync(Team team)
        {
            team.DateUpdated = DateTime.UtcNow;
            await _repo.UpdateAsync(team);
        }

        public async Task DeleteTeamCascadeAsync(string teamId)
        {
            await _repo.DeleteTeamCascadeAsync(teamId);
        }

        public async Task<Team> CreateTeamAsync(CreateTeamDto dto, string userId)
        {
            // 1. Chuyển đổi DTO -> Entity Team (Mapping)
            var newTeam = new Team
            {
                TeamId = Guid.NewGuid().ToString(),
                Name = dto.Name,
                Description = dto.Description,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow
            };

            // 2. Gọi Repo để lưu Team
            await _repo.AddTeamAsync(newTeam);

            // 3. Logic nghiệp vụ: Người tạo auto là Owner
            var member = new TeamMember
            {
                TeamId = newTeam.TeamId,
                UserId = userId,
                Role = "Owner",
                DateJoined = DateTime.UtcNow
            };

            // 4. Gọi Repo để lưu Member
            await _repo.AddTeamMemberAsync(member);

            return newTeam;
        }

        public async Task<bool> AddUserToTeamAsync(string teamId, string email, string role)
        {
            // 1. Logic: Tìm User xem có tồn tại không
            var user = await _repo.GetUserByEmailAsync(email);
            if (user == null)
            {
                throw new Exception("Email không tồn tại trong hệ thống.");
            }

            // 2. Logic: Kiểm tra xem đã vào nhóm chưa (tránh trùng lặp)
            var isExist = await _repo.IsMemberExistAsync(teamId, user.UserId);
            if (isExist)
            {
                throw new Exception("Thành viên này đã ở trong nhóm rồi.");
            }

            // 3. Logic: Chuẩn bị dữ liệu để lưu
            var newMember = new TeamMember
            {
                TeamId = teamId,
                UserId = user.UserId,
                Role = role,
                DateJoined = DateTime.UtcNow
            };

            // 4. Gọi Repo để lưu xuống DB
            await _repo.AddMemberAsync(newMember);
            return true;
        }

        public Task<List<TeamBriefDto>> GetTeamsByUserIdAsync(string userId)
        {
            return _repo.GetTeamsByUserIdAsync(userId);
        }
    }
}
