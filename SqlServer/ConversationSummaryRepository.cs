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
    public class ConversationSummaryRepository : IConversationSummaryRepository
    {
        private readonly ApplicationDbContext _db;

        public ConversationSummaryRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<ConversationSummary?> GetAsync(Guid conversationId)
        {
            return await _db.Set<ConversationSummary>()
                .FirstOrDefaultAsync(x => x.ConversationId == conversationId);
        }

        public async Task UpsertAsync(ConversationSummary summary)
        {
            var existing = await GetAsync(summary.ConversationId);
            if (existing == null)
            {
                await _db.Set<ConversationSummary>().AddAsync(summary);
            }
            else
            {
                existing.Summary = summary.Summary;
                existing.DateUpdated = summary.DateUpdated;
                _db.Set<ConversationSummary>().Update(existing);
            }

            await _db.SaveChangesAsync();
        }

        public async Task<int> CountMessagesAsync(Guid conversationId)
        {
            return await _db.Set<ChatMessage>()
                .CountAsync(m => m.ConversationId == conversationId);
        }
    }
}
