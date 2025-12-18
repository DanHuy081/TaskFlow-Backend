using CoreEntities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.Repository
{
    public interface IChatRepository
    {
        Task<List<Conversation>> GetUserConversationsAsync(string userId);
        Task<Conversation> GetConversationByIdAsync(Guid id, string userId);
        Task CreateConversationAsync(Conversation conversation);
        Task UpdateConversationAsync(Conversation conversation); // Để update Title, DateUpdated
        Task AddMessageAsync(ChatMessage message);
        Task DeleteConversationAsync(Guid id);
        Task<Conversation> GetConversationByIdRawAsync(Guid conversationId);
        Task DeleteConversationAsync(Guid conversationId, string userId);
    }
}
