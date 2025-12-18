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
    public class ChatRepository : IChatRepository
    {
        private readonly ApplicationDbContext _context; // Thay AppDbContext bằng tên DbContext của bạn

        public ChatRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Conversation>> GetUserConversationsAsync(string userId)
        {
            return await _context.Conversations
                .Where(c => c.UserId == userId && c.IsActive)
                .OrderByDescending(c => c.DateUpdated)
                .ToListAsync();
        }

        public async Task<Conversation> GetConversationByIdAsync(Guid id, string userId)
        {
            return await _context.Conversations
                .Include(c => c.Messages) // Load luôn tin nhắn cũ
                .FirstOrDefaultAsync(c => c.ConversationId == id && c.UserId == userId);
        }

        public async Task CreateConversationAsync(Conversation conversation)
        {
            await _context.Conversations.AddAsync(conversation);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateConversationAsync(Conversation conversation)
        {
            _context.Conversations.Update(conversation);
            await _context.SaveChangesAsync();
        }

        public async Task AddMessageAsync(ChatMessage message)
        {
            await _context.ChatMessages.AddAsync(message);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteConversationAsync(Guid id)
        {
            var convo = await _context.Conversations.FindAsync(id);
            if (convo != null)
            {
                _context.Conversations.Remove(convo);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Conversation> GetConversationByIdRawAsync(Guid conversationId)
        {
            return await _context.Conversations
                .Include(c => c.Messages)
                .FirstAsync(c => c.ConversationId == conversationId);
        }

        public async Task DeleteConversationAsync(Guid conversationId, string userId)
        {
            var conv = await _context.Conversations
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.ConversationId == conversationId && c.UserId == userId);

            if (conv == null) return;

            // Xoá messages trước để tránh FK constraint
            if (conv.Messages != null && conv.Messages.Count > 0)
                _context.ChatMessages.RemoveRange(conv.Messages);

            _context.Conversations.Remove(conv);
            await _context.SaveChangesAsync();
        }

    }
}
