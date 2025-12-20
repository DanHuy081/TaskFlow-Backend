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
    public class FolderRepository : IFolderRepository
    {
        private readonly ApplicationDbContext _context;

        public FolderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Folder>> GetAllAsync()
        {
            return await _context.Folders.ToListAsync();
        }

        public async Task<Folder> GetByIdAsync(string id)
        {
            return await _context.Folders.FindAsync(id);
        }

        public async Task AddAsync(Folder folder)
        {
            _context.Folders.Add(folder);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Folder folder)
        {
            _context.Folders.Update(folder);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var folder = await _context.Folders.FindAsync(id);
            if (folder != null)
            {
                _context.Folders.Remove(folder);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<FolderBriefDto>> GetFoldersBySpaceIdAsync(string spaceId)
        {
            return await _context.Set<Folder>()
                .Where(f => f.SpaceId.ToString() == spaceId)
                .Select(f => new FolderBriefDto
                {
                    FolderId = f.FolderId.ToString(),
                    SpaceId = f.SpaceId.ToString(),
                    Name = f.Name
                })
                .ToListAsync();
        }

        public async Task<List<FolderBriefDto>> GetFoldersByUserIdAsync(string userId)
        {
            // user -> teammembers -> spaces -> folders
            var data = await (
                from tm in _context.Set<TeamMember>()
                join s in _context.Set<CoreEntities.Model.Space>() on tm.TeamId equals s.TeamId
                join f in _context.Set<Folder>() on s.SpaceId equals f.SpaceId
                where tm.UserId == userId
                select new FolderBriefDto
                {
                    FolderId = f.FolderId.ToString(),
                    SpaceId = f.SpaceId.ToString(),
                    Name = f.Name
                }
            ).Distinct().ToListAsync();

            return data;
        }
    }
}

