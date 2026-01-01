using CoreEntities.Model;
using LogicBusiness.Repository;
using Microsoft.EntityFrameworkCore;
using SqlServer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
            if (string.IsNullOrWhiteSpace(query)) return new List<KnowledgeChunk>();

            // 1. Tách từ khóa từ câu hỏi (Bỏ dấu câu, chuyển thường)
            // Ví dụ: "Làm sao tạo task?" -> ["làm", "sao", "tạo", "task"]
            var keywords = Regex.Split(query.ToLower(), @"\W+")
                                .Where(w => w.Length > 2) // Bỏ từ quá ngắn (là, ở...)
                                .Distinct()
                                .ToList();

            if (!keywords.Any()) return new List<KnowledgeChunk>();

            // 2. Lấy dữ liệu từ DB
            // Lưu ý: Nếu DB lớn (>10k dòng), cần dùng Full-Text Search của SQL Server.
            // Với quy mô nhỏ/vừa, ta lấy về RAM để tính điểm cho chính xác.
            var allChunks = await _db.Set<KnowledgeChunk>()
                                     .AsNoTracking()
                                     .ToListAsync();

            // 3. Tính điểm độ phù hợp (Scoring)
            var results = allChunks.Select(chunk =>
            {
                int score = 0;
                string titleLower = (chunk.Title ?? "").ToLower();
                string contentLower = (chunk.Content ?? "").ToLower();
                string tagsLower = (chunk.Tags ?? "").ToLower();

                foreach (var word in keywords)
                {
                    // Từ khóa xuất hiện ở Tiêu đề: +3 điểm
                    if (titleLower.Contains(word)) score += 3;

                    // Từ khóa xuất hiện ở Tags: +2 điểm
                    if (tagsLower.Contains(word)) score += 2;

                    // Từ khóa xuất hiện ở Nội dung: +1 điểm
                    if (contentLower.Contains(word)) score += 1;
                }

                return new { Chunk = chunk, Score = score };
            })
            .Where(x => x.Score > 0) // Chỉ lấy cái nào có liên quan
            .OrderByDescending(x => x.Score) // Điểm cao xếp trước
            .Take(take)
            .Select(x => x.Chunk)
            .ToList();

            return results;
        }

        public async Task<List<KnowledgeChunk>> SearchByTagsAsync(IEnumerable<string> tags, int take = 5)
        {
            var tagList = tags?
                .Select(t => (t ?? "").Trim().ToLowerInvariant())
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Distinct()
                .ToList() ?? new List<string>();

            if (tagList.Count == 0) return new List<KnowledgeChunk>();

            return await _db.Set<KnowledgeChunk>()
                .AsNoTracking()
                .Where(k => tagList.Any(t => k.Tags.ToLower().Contains(t)))
                .OrderByDescending(k => k.DateUpdated)
                .Take(take)
                .ToListAsync();
        }
    }
}