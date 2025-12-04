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

        public async Task DeleteAsync(string id)
        {
            var space = await _context.Spaces.FindAsync(id);
            if (space != null)
            {
                _context.Spaces.Remove(space);
                await _context.SaveChangesAsync();
            }
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
    }
}
