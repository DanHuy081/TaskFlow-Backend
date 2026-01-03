using CoreEntities.Model.DTOs;
using CoreEntities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.UseCase
{
    public interface IChatService
    {
        // Xử lý chat: Nhận request -> Gọi AI -> Lưu DB -> Trả về response
        Task<ChatResponseDto> ProcessChatAsync(ChatRequestDto request, string userId);

        // Lấy lịch sử chat
        Task<List<Conversation>> GetHistoryAsync(string userId);

        Task DeleteConversationAsync(Guid conversationId, string userId);

        Task<Conversation> GetConversationAsync(Guid conversationId, string userId);

        Task<string> ProcessPublicChatAsync(string message, string sessionId);
    }
}

