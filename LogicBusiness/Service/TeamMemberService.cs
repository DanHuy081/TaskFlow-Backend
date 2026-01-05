using CoreEntities.Model;
using CoreEntities.Model.DTOs;
using CoreEntities.Model.Enums;
using LogicBusiness.Repository;
using LogicBusiness.UseCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.Service
{
    public class TeamMemberService : ITeamMemberService
    {
        private readonly ITeamMemberRepository _repo;

        public TeamMemberService(ITeamMemberRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<TeamMember>> GetAllAsync()
            => await _repo.GetAllAsync();

        public async Task<IEnumerable<TeamMember>> GetByTeamIdAsync(string teamId)
            => await _repo.GetByTeamIdAsync(teamId);

        public async Task<IEnumerable<TeamMember>> GetByUserIdAsync(string userId)
            => await _repo.GetByUserIdAsync(userId);

        public async Task<TeamMember> GetAsync(string teamId, string userId)
            => await _repo.GetAsync(teamId, userId);

        public async Task AddAsync(TeamMember member)
        {
            member.DateJoined = DateTime.UtcNow;
            await _repo.AddAsync(member);
        }

        public async Task UpdateAsync(TeamMember member)
            => await _repo.UpdateAsync(member);

        public async Task DeleteAsync(string teamId, string userId)
            => await _repo.DeleteAsync(teamId, userId);

        public async Task<TeamMemberUpdateRoleDto> GetPermissionsAsync(string teamId, string userId)
        {
            var member = await _repo.GetMemberAsync(teamId, userId);
            if (member == null) return null;

            // Map Entity -> DTO
            return MapToDto(member);
        }

        public async Task<TeamMemberUpdateRoleDto> UpdatePermissionsAsync(TeamMemberUpdateRoleDto request)
        {
            var member = await _repo.GetMemberAsync(request.TeamId, request.UserId);
            if (member == null) throw new Exception("Member not found");

            // Cập nhật giá trị từ Request vào Entity
            member.CanCreateTasks = request.CanCreateTasks;
            member.CanEditTasks = request.CanEditTasks;
            member.CanDeleteTasks = request.CanDeleteTasks;
            member.CanSetTaskDueDate = request.CanSetTaskDueDate;
            member.CanChangeTaskPriority = request.CanChangeTaskPriority;
            member.CanChangeTaskStatus = request.CanChangeTaskStatus;
            member.CanAssignTasks = request.CanAssignTasks;
            member.CanCommentOnTasks = request.CanCommentOnTasks;
            member.CanUploadFiles = request.CanUploadFiles;

            // Lưu xuống DB
            await _repo.UpdateMemberAsync(member);

            // QUAN TRỌNG: Trả về DTO mới nhất để Frontend cập nhật State
            return MapToDto(member);
        }

        // Hàm phụ trợ để map đỡ code lặp lại
        private TeamMemberUpdateRoleDto MapToDto(TeamMember member)
        {
            return new TeamMemberUpdateRoleDto
            {
                UserId = member.UserId,
                TeamId = member.TeamId,
                Role = member.Role,
                CanCreateTasks = member.CanCreateTasks,
                CanEditTasks = member.CanEditTasks,
                CanDeleteTasks = member.CanDeleteTasks,
                CanSetTaskDueDate = member.CanSetTaskDueDate,
                CanChangeTaskPriority = member.CanChangeTaskPriority,
                CanChangeTaskStatus = member.CanChangeTaskStatus,
                CanAssignTasks = member.CanAssignTasks,
                CanCommentOnTasks = member.CanCommentOnTasks,
                CanUploadFiles = member.CanUploadFiles
            };
        }
        public async Task<TeamRole?> GetUserRoleAsync(string userId, string teamId)
        {
            // Service gọi xuống Repository để lấy dữ liệu
            return await _repo.GetUserRoleAsync(userId, teamId);
        }
    }
}
