using CoreEntities.Model;
using LogicBusiness.Repository;
using Microsoft.EntityFrameworkCore;
using SqlServer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlServer
{
    public class KnowledgeRepository : IKnowledgeRepository
    {
        private readonly ApplicationDbContext _db;

        public KnowledgeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<KnowledgeChunk>> SearchAsync(string query, int take = 5)
        {
            query = (query ?? "").Trim();
            if (string.IsNullOrWhiteSpace(query)) return new List<KnowledgeChunk>();

            // search Title, Tags, Content
            return await _db.Set<KnowledgeChunk>()
                .Where(k =>
                    k.Title.Contains(query) ||
                    k.Tags.Contains(query) ||
                    k.Content.Contains(query))
                .OrderByDescending(k => k.DateUpdated)
                .Take(take)
                .ToListAsync();
        }

        public async Task<List<KnowledgeChunk>> SearchByTagsAsync(IEnumerable<string> tags, int take = 5)
        {
            var tagList = tags?
                .Select(t => (t ?? "").Trim().ToLowerInvariant())
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Distinct()
                .ToList() ?? new List<string>();

            if (tagList.Count == 0) return new List<KnowledgeChunk>();

            // Tags lưu dạng "schema,db,project"
            return await _db.Set<KnowledgeChunk>()
                .Where(k => tagList.Any(t => k.Tags.ToLower().Contains(t)))
                .OrderByDescending(k => k.DateUpdated)
                .Take(take)
                .ToListAsync();
        }
    }
}
