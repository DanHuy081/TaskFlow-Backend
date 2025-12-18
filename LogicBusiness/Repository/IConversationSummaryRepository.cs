using CoreEntities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.Repository
{
    public interface IConversationSummaryRepository
    {
        Task<ConversationSummary?> GetAsync(Guid conversationId);
        Task UpsertAsync(ConversationSummary summary);
        Task<int> CountMessagesAsync(Guid conversationId);
    }
}
