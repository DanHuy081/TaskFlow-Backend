using CoreEntities.Model;
using LogicBusiness.Repository;
using LogicBusiness.UseCase;
using Mscc.GenerativeAI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicBusiness.Service
{
    public class SummaryService : ISummaryService
    {
        private readonly IConversationSummaryRepository _summaryRepo;
        private readonly IChatRepository _chatRepo;
        private readonly GenerativeModel _model;

        // cập nhật mỗi 8 messages (bạn có thể đổi)
        private const int UpdateEveryNMessages = 8;

        public SummaryService(
            IConversationSummaryRepository summaryRepo,
            IChatRepository chatRepo,
             IAIModelFactory modelFactory)
        {
            _summaryRepo = summaryRepo;
            _chatRepo = chatRepo;
            _model = modelFactory.Create();
        }

        public async Task<string?> GetSummaryAsync(Guid conversationId)
        {
            var s = await _summaryRepo.GetAsync(conversationId);
            return s?.Summary;
        }

        public async Task UpdateSummaryIfNeededAsync(Guid conversationId)
        {
            var count = await _summaryRepo.CountMessagesAsync(conversationId);
            if (count == 0 || count % UpdateEveryNMessages != 0) return;

            // lấy 20 messages gần nhất để tóm tắt (đủ ngữ cảnh)
            var convo = await _chatRepo.GetConversationByIdRawAsync(conversationId);
            // NOTE: nếu repo bạn không có hàm này, xem mục 7.3 bên dưới

            var recent = (convo.Messages ?? new List<CoreEntities.Model.ChatMessage>())
                .OrderByDescending(m => m.DateCreated)
                .Take(20)
                .OrderBy(m => m.DateCreated)
                .ToList();

            var sb = new StringBuilder();
            sb.AppendLine("Hãy tóm tắt hội thoại sau thành 6-10 dòng, giữ lại:");
            sb.AppendLine("- Mục tiêu người dùng");
            sb.AppendLine("- Team/space/list/task đang nói tới (nếu có)");
            sb.AppendLine("- Các quyết định quan trọng, ràng buộc");
            sb.AppendLine("- Việc còn dang dở");
            sb.AppendLine("Không bịa dữ liệu.");
            sb.AppendLine();
            sb.AppendLine("HỘI THOẠI:");
            foreach (var m in recent)
            {
                var who = (m.Role ?? "").Equals("user", StringComparison.OrdinalIgnoreCase) ? "User" : "Assistant";
                sb.AppendLine($"{who}: {m.Content}");
            }

            var resp = await _model.GenerateContent(sb.ToString());
            var summary = resp?.Text?.Trim();
            if (string.IsNullOrWhiteSpace(summary)) return;

            await _summaryRepo.UpsertAsync(new ConversationSummary
            {
                ConversationId = conversationId,
                Summary = summary,
                DateUpdated = DateTime.UtcNow
            });
        }
    }
}
