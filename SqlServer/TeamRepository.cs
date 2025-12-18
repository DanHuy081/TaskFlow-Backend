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

namespace SqlServer
{
    public class TeamRepository : ITeamRepository
    {
        private readonly ApplicationDbContext _context;

        public TeamRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Team>> GetAllAsync()
        {
            return await _context.Teams.ToListAsync();
        }

        public async Task<Team> GetByIdAsync(string id)
        {
            return await _context.Teams.FindAsync(id);
        }

        public async Task AddAsync(Team team)
        {
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Team team)
        {
            _context.Teams.Update(team);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTeamCascadeAsync(string teamId)
        {
            // lấy tất cả spaces thuộc team
            var spaces = await _context.Spaces
                .Where(s => s.TeamId == teamId)
                .ToListAsync();

            foreach (var space in spaces)
            {
                // 1. Lấy folders
                var folders = await _context.Folders
                    .Where(f => f.SpaceId == space.SpaceId)
                    .ToListAsync();

                foreach (var folder in folders)
                {
                    // 2. Lấy lists
                    var lists = await _context.Lists
                        .Where(l => l.FolderId == folder.FolderId)
                        .ToListAsync();

                    foreach (var list in lists)
                    {
                        // 3. Lấy tasks
                        var tasks = await _context.Tasks
                            .Where(t => t.ListId == list.ListId)
                            .ToListAsync();

                        foreach (var task in tasks)
                        {
                            // Xóa checklist items
                            var checklistItems = await _context.ChecklistItems
                                .Where(ci => ci.Checklist.TaskId == task.Id)
                                .ToListAsync();
                            _context.ChecklistItems.RemoveRange(checklistItems);

                            // Xóa checklists
                            var checklists = await _context.Checklists
                                .Where(c => c.TaskId == task.Id)
                                .ToListAsync();
                            _context.Checklists.RemoveRange(checklists);

                            // Xóa comments
                            var comments = await _context.Comments
                                .Where(c => c.TaskId == task.Id)
                                .ToListAsync();
                            _context.Comments.RemoveRange(comments);

                            // Xóa attachments
                            var attachments = await _context.Attachments
                                .Where(a => a.TaskId == task.Id)
                                .ToListAsync();
                            _context.Attachments.RemoveRange(attachments);

                            // Xóa task assignees
                            var assignees = await _context.TaskAssignees
                                .Where(a => a.TaskId == task.Id)
                                .ToListAsync();
                            _context.TaskAssignees.RemoveRange(assignees);

                            // Xóa custom field values
                            var customValues = await _context.TaskCustomFieldValues
                                .Where(cv => cv.TaskId == task.Id)
                                .ToListAsync();
                            _context.TaskCustomFieldValues.RemoveRange(customValues);

                            // Xóa time entries
                            var timeEntries = await _context.TimeEntries
                                .Where(te => te.TaskId == task.Id)
                                .ToListAsync();
                            _context.TimeEntries.RemoveRange(timeEntries);

                            // Cuối cùng xóa TASK
                            _context.Tasks.Remove(task);
                        }   

                        // Xóa LIST
                        _context.Lists.RemoveRange(lists);
                    }

                    // Xóa FOLDER
                    _context.Folders.Remove(folder);
                }

                // Xóa SPACE
                _context.Spaces.Remove(space);
            }

            // Xoá goals
            var goals = await _context.Goals.Where(g => g.TeamId == teamId).ToListAsync();
            _context.Goals.RemoveRange(goals);

            // Xóa team members
            var members = await _context.TeamMembers.Where(tm => tm.TeamId == teamId).ToListAsync();
            _context.TeamMembers.RemoveRange(members);

            // Cuối cùng xoá TEAM
            var team = await _context.Teams.FindAsync(teamId);
            if (team != null)
            {
                _context.Teams.Remove(team);
            }

            await _context.SaveChangesAsync();
        }


        public async Task AddTeamAsync(Team team)
        {
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
        }

        public async Task AddTeamMemberAsync(TeamMember member)
        {
            _context.TeamMembers.Add(member);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsMemberExistAsync(string teamId, string userId)
        {
            return await _context.TeamMembers
                .AnyAsync(tm => tm.TeamId == teamId && tm.UserId == userId);
        }

        public async Task AddMemberAsync(TeamMember member)
        {
            await _context.TeamMembers.AddAsync(member);
            await _context.SaveChangesAsync();
        }

        public async Task<UserFL?> GetUserByEmailAsync(string email)
        {
            return await _context.UserFLs.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<List<TeamBriefDto>> GetTeamsByUserIdAsync(string userId)
        {
            var data = await (
                from tm in _context.Set<TeamMember>()
                join t in _context.Set<Team>() on tm.TeamId equals t.TeamId
                where tm.UserId == userId
                select new TeamBriefDto
                {
                    TeamId = t.TeamId.ToString(),
                    Name = t.Name
                }
            ).Distinct().ToListAsync();

            return data;
        }
    }
}
