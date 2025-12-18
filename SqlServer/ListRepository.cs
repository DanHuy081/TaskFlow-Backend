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
using System.Collections;

namespace SqlServer
{
    public class ListRepository : IListRepository
    {
        private readonly ApplicationDbContext _context;

        public ListRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<List>> GetAllAsync()
        {
            return await _context.Lists.ToListAsync();
        }

        public async Task<List> GetByIdAsync(string id)
        {
            return await _context.Lists.FindAsync(id);
        }

        public async Task<List> CreateAsync(List entity)
        {
            _context.Lists.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(List list)
        {
            _context.Lists.Update(list);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var list = await _context.Lists.FindAsync(id);
            if (list != null)
            {
                _context.Lists.Remove(list);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<List>> GetBySpaceIdAsync(string spaceId)
        {
            return await _context.Lists
                .Where(l => l.SpaceId == spaceId)
                .ToListAsync();
        }

        public async Task<List<List>> GetByFolderIdAsync(string folderId)
        {
            return await _context.Lists
                .Where(l => l.FolderId == folderId)
                .ToListAsync();
        }

        public async Task<List<ListBriefDto>> GetListsBySpaceIdAsync(string spaceId)
        {
            return await _context.Set<List>()
                .Where(l => l.SpaceId.ToString() == spaceId)
                .Select(l => new ListBriefDto
                {
                    ListId = l.ListId.ToString(),
                    SpaceId = l.SpaceId.ToString(),
                    Name = l.Name
                })
                .ToListAsync();
        }

        public async Task<List<ListBriefDto>> GetListsByUserIdAsync(string userId)
        {
            // user -> teammembers -> spaces -> lists
            var data = await (
                from tm in _context.Set<TeamMember>()
            join s in _context.Set<Space>() on tm.TeamId equals s.TeamId
                join l in _context.Set<List>() on s.SpaceId equals l.SpaceId
                where tm.UserId == userId
                select new ListBriefDto
                {
                    ListId = l.ListId.ToString(),
                    SpaceId = s.SpaceId.ToString(),
                    Name = l.Name
                }
            ).Distinct().ToListAsync();

            return data;
        }
    }
}
