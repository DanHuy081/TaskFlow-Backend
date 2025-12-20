using Space = CoreEntities.Model.Space;
using LogicBusiness.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlServer.Data;
using CoreEntities.Model.DTOs;
using CoreEntities.Model;

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

        public async Task CreateAsync(Space space)
        {
            _context.Spaces.Add(space);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Space space)
        {
            _context.Spaces.Update(space);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSpaceCascadeAsync(string spaceId)
        {
            // Lấy space trước
            var space = await _context.Spaces.FirstOrDefaultAsync(s => s.SpaceId == spaceId);
            if (space == null) return;

            // ------------------------------------------------------------
            // 1. Lấy tất cả folders thuộc space
            // ------------------------------------------------------------
            var folders = await _context.Folders
                .Where(f => f.SpaceId == spaceId)
                .ToListAsync();

            foreach (var folder in folders)
            {
                // ------------------------------------------------------------
                // 2. Lấy tất cả lists trong folder
                // ------------------------------------------------------------
                var lists = await _context.Lists
                    .Where(l => l.FolderId == folder.FolderId)
                    .ToListAsync();

                foreach (var list in lists)
                {
                    // ------------------------------------------------------------
                    // 3. Lấy tasks trong list
                    // ------------------------------------------------------------
                    var tasks = await _context.Tasks
                        .Where(t => t.ListId == list.ListId)
                        .ToListAsync();

                    foreach (var task in tasks)
                    {
                        // Checklist Items
                        var checklistItems = await _context.ChecklistItems
                            .Where(ci => ci.Checklist.TaskId == task.Id)
                            .ToListAsync();
                        _context.ChecklistItems.RemoveRange(checklistItems);

                        // Checklists
                        var checklists = await _context.Checklists
                            .Where(c => c.TaskId == task.Id)
                            .ToListAsync();
                        _context.Checklists.RemoveRange(checklists);

                        // Comments
                        var comments = await _context.Comments
                            .Where(c => c.TaskId == task.Id)
                            .ToListAsync();
                        _context.Comments.RemoveRange(comments);

                        // Attachments
                        var attachments = await _context.Attachments
                            .Where(a => a.TaskId == task.Id)
                            .ToListAsync();
                        _context.Attachments.RemoveRange(attachments);

                        // Assignees
                        var assignees = await _context.TaskAssignees
                            .Where(a => a.TaskId == task.Id)
                            .ToListAsync();
                        _context.TaskAssignees.RemoveRange(assignees);

                        // Time Entries
                        var timeEntries = await _context.TimeEntries
                            .Where(te => te.TaskId == task.Id)
                            .ToListAsync();
                        _context.TimeEntries.RemoveRange(timeEntries);

                        // Custom Field Values
                        var customValues = await _context.TaskCustomFieldValues
                            .Where(cv => cv.TaskId == task.Id)
                            .ToListAsync();
                        _context.TaskCustomFieldValues.RemoveRange(customValues);

                        // Cuối cùng xoá task
                        _context.Tasks.Remove(task);
                    }

                    // ------------------------------------------------------------
                    // Xóa List
                    // ------------------------------------------------------------
                    _context.Lists.Remove(list);
                }

                // ------------------------------------------------------------
                // Xóa Folder
                // ------------------------------------------------------------
                _context.Folders.Remove(folder);
            }

            // ------------------------------------------------------------
            // Cuối cùng xóa Space
            // ------------------------------------------------------------
            _context.Spaces.Remove(space);

            await _context.SaveChangesAsync();
        }


        public async Task<List<Space>> GetSpacesByUserAsync(string userId, string? teamId = null)
        {
            // Khởi tạo truy vấn
            var query = _context.Spaces
                .Include(s => s.Teams) // Load thông tin Team để check bảo mật
                .AsQueryable();

            // 1. ĐIỀU KIỆN BẢO MẬT (Bắt buộc):
            // Chỉ lấy Space thuộc về những Team mà User này CÓ tham gia.
            // (Ngăn chặn việc User A xem trộm data của Team mà họ không tham gia)
            query = query.Where(s => s.Teams.TeamMembers.Any(tm => tm.UserId == userId));

            // 2. ĐIỀU KIỆN LỌC THEO TEAM (Đây là cái bạn đang thiếu):
            // Nếu Frontend gửi teamId lên, thì CHỈ lấy Space của đúng Team đó.
            if (!string.IsNullOrEmpty(teamId))
            {
                query = query.Where(s => s.TeamId == teamId);
            }

            // Thực thi và trả về
            return await query.ToListAsync();
        }

        public async Task<List<SpaceBriefDto>> GetSpacesByUserIdAsync(string userId)
        {
            // user -> teammembers -> spaces
            var data = await (
                from tm in _context.Set<TeamMember>()
                join s in _context.Set<Space>() on tm.TeamId equals s.TeamId
                where tm.UserId == userId
                select new SpaceBriefDto
                {
                    SpaceId = s.SpaceId.ToString(),
                    TeamId = s.TeamId.ToString(),
                    Name = s.Name
                }
            ).Distinct().ToListAsync();

            return data;
        }

        public async Task<List<SpaceBriefDto>> GetSpacesByTeamIdAsync(string teamId)
        {
            // teamId string -> compare theo ToString() cho nhanh (hoặc parse Guid nếu bạn muốn)
            var data = await _context.Set<Space>()
                .Where(s => s.TeamId.ToString() == teamId)
                .Select(s => new SpaceBriefDto
                {
                    SpaceId = s.SpaceId.ToString(),
                    TeamId = s.TeamId.ToString(),
                    Name = s.Name
                })
                .ToListAsync();

            return data;
        }

        public async Task<SpaceBriefDto?> GetSpaceByIdAsync(string spaceId)
        {
            return await _context.Set<Space>()
                .Where(s => s.SpaceId.ToString() == spaceId)
                .Select(s => new SpaceBriefDto
                {
                    SpaceId = s.SpaceId.ToString(),
                    TeamId = s.TeamId.ToString(),
                    Name = s.Name
                })
                .FirstOrDefaultAsync();
        }

        public async Task<PersonalSpaceDto> GetOrCreatePersonalSpaceAsync(string userIdString)
        {
            // 1. Chuyển đổi UserId từ string sang Guid để khớp với DB
            if (!Guid.TryParse(userIdString, out Guid userGuid))
            {
                throw new ArgumentException("UserId không hợp lệ");
            }

            // 2. Tìm Space có cờ IsPersonal = true của User này
            var personalSpace = await _context.Spaces
                .FirstOrDefaultAsync(s => s.UserId == userGuid && s.IsPersonal == true);

            // 3. Nếu chưa có -> TỰ ĐỘNG TẠO (Auto Create)
            if (personalSpace == null)
            {
                // Tạo Space mới
                personalSpace = new Space
                {
                    SpaceId = Guid.NewGuid().ToString(), // DB là nvarchar nên để ToString()
                    Name = "Không gian cá nhân",
                    UserId = userGuid, // DB là uniqueidentifier
                    TeamId = null,     // Không thuộc team nào
                    IsPersonal = true, // Đánh dấu là Personal
                    IsPrivate = true,
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    Color = "#8e44ad" // Màu tím giống hình
                };
                _context.Spaces.Add(personalSpace);

                // TẠO LUÔN 1 LIST MẶC ĐỊNH (Bắt buộc phải có List mới tạo được Task)
                var defaultList = new List // Thay 'ListEntity' bằng tên class List của bạn
                {
                    ListId = Guid.NewGuid().ToString(),
                    Name = "Công việc của tôi",
                    SpaceId = personalSpace.SpaceId,
                    DateCreated = DateTime.Now
                    // Nếu bảng List có UserId thì gán thêm userGuid vào
                };
                _context.Lists.Add(defaultList);

                await _context.SaveChangesAsync();
            }

            // 4. Tính toán số liệu thống kê (Để hiển thị Header)

            // Tìm ListId mặc định để trả về cho FE
            // (Tìm list đầu tiên trong Space này)
            var defaultListId = await _context.Lists
                .Where(l => l.SpaceId == personalSpace.SpaceId)
                .Select(l => l.ListId)
                .FirstOrDefaultAsync();

            // Đếm Task: Join từ Task -> List -> Space
            // Giả sử bảng Task có cột ListId
            var queryTasks = _context.Tasks
                .Where(t => t.List.SpaceId == personalSpace.SpaceId);

            int total = await queryTasks.CountAsync();
            int completed = await queryTasks.CountAsync(t => t.Status == "DONE" || t.Status == "COMPLETE");

            return new PersonalSpaceDto
            {
                SpaceId = personalSpace.SpaceId,
                SpaceName = personalSpace.Name,
                DefaultListId = defaultListId,
                TotalTasks = total,
                CompletedTasks = completed,
                CompletionPercentage = total == 0 ? 0 : Math.Round((double)completed / total * 100, 1)
            };
        }
    }
}

