using CoreEntities.Model;
using CoreEntities.Model.DTOs;
using LogicBusiness.Repository;
using LogicBusiness.UseCase;
using Mscc.GenerativeAI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using DBChatMessage = CoreEntities.Model.ChatMessage;

namespace LogicBusiness.Service
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly ITaskService _taskService;
        private readonly GenerativeModel _model;
        private readonly IKnowledgeRepository _knowledgeRepository;
        private readonly ISummaryService _summaryService;
        private readonly ITeamService _teamService;
        private readonly ISpaceService _spaceService;
        private readonly IListService _listService;

        public ChatService(
            IChatRepository chatRepository,
            ITaskService taskService,
            IKnowledgeRepository knowledgeRepository,
            IAIModelFactory modelFactory,
            ISummaryService summaryService,
            ITeamService teamService,
            ISpaceService spaceService,
            IListService listService)
        {
            _chatRepository = chatRepository;
            _taskService = taskService;
            _knowledgeRepository = knowledgeRepository;

            _model = modelFactory.Create();

            _summaryService = summaryService;
            _teamService = teamService;
            _spaceService = spaceService;
            _listService = listService;
        }

        public async Task<List<Conversation>> GetHistoryAsync(string userId)
        {
            return await _chatRepository.GetUserConversationsAsync(userId);
        }

        public async Task<ChatResponseDto> ProcessChatAsync(ChatRequestDto request, string userId)
        {
            // 1) Get/Create Conversation
            Conversation conversation;
            if (request.ConversationId == null)
            {
                conversation = new Conversation
                {
                    ConversationId = Guid.NewGuid(),
                    UserId = userId,
                    TeamId = request.CurrentTeamId,
                    Title = "New Chat",
                    IsActive = true
                };
                await _chatRepository.CreateConversationAsync(conversation);
            }
            else
            {
                conversation = await _chatRepository.GetConversationByIdAsync(request.ConversationId.Value, userId);
                if (conversation == null) throw new KeyNotFoundException("Conversation not found");
            }

            // 2) Save User Message
            var userMsg = new DBChatMessage
            {
                MessageId = Guid.NewGuid(),
                ConversationId = conversation.ConversationId,
                Role = "user",
                Content = request.Message
            };
            await _chatRepository.AddMessageAsync(userMsg);

            // 3) Retrieve knowledge (RAG)
            var knowledge = await _knowledgeRepository.SearchAsync(request.Message, take: 3);

            // 4) Build prompt (có currentSpaceId để trả lời theo đúng UI)
            var prompt = await BuildPromptAsync(
                conversationId: conversation.ConversationId,
                conversation: conversation,
                userMessage: request.Message,
                userId: userId,
                knowledge: knowledge,
                currentSpaceId: request.CurrentSpaceId
            );

            // 5) Call Gemini
            var response = await _model.GenerateContent(prompt);
            var aiReply = response?.Text;
            if (string.IsNullOrWhiteSpace(aiReply))
                aiReply = "Đã xử lý xong yêu cầu.";

            // 6) Save AI Reply
            var aiMessage = new DBChatMessage
            {
                MessageId = Guid.NewGuid(),
                ConversationId = conversation.ConversationId,
                Role = "model",
                Content = aiReply
            };
            await _chatRepository.AddMessageAsync(aiMessage);

            // ✅ Update summary sau khi đã có cả user + assistant
            await _summaryService.UpdateSummaryIfNeededAsync(conversation.ConversationId);

            // 7) Update Title
            if (conversation.Title == "New Chat")
            {
                conversation.Title = request.Message.Length > 40
                    ? request.Message.Substring(0, 40) + "..."
                    : request.Message;

                await _chatRepository.UpdateConversationAsync(conversation);
            }

            return new ChatResponseDto
            {
                ConversationId = conversation.ConversationId,
                Reply = aiReply,
                Title = conversation.Title
            };
        }

        // ---------------- Helpers ----------------

        private async Task<string> BuildPromptAsync(
            Guid conversationId,
            Conversation conversation,
            string userMessage,
            string userId,
            List<KnowledgeChunk> knowledge,
            Guid? currentSpaceId)
        {
            // Lấy tối đa 8 message gần nhất để giảm prompt
            var msgs = (conversation.Messages ?? new List<DBChatMessage>())
                .OrderBy(m => m.DateCreated)
                .TakeLast(8)
                .ToList();

            var sb = new StringBuilder();

            sb.AppendLine("Bạn là trợ lý cho ứng dụng quản lý công việc TaskFlow.");
            sb.AppendLine("Trả lời tiếng Việt, rõ ràng, ngắn gọn. Nếu cần, liệt kê bullet.");
            sb.AppendLine("Bạn được cung cấp dữ liệu nội bộ trong prompt. Không được nói 'tôi không truy cập được dữ liệu hệ thống/tài khoản'. Nếu thiếu dữ liệu, hãy hỏi lại 1 câu cụ thể.");
            sb.AppendLine("QUY TẮC: Nếu có 'NGỮ CẢNH UI HIỆN TẠI', hãy ưu tiên trả lời theo Space đó. Chỉ khi người dùng hỏi phạm vi khác thì mới mở rộng.");
            sb.AppendLine();

            // 1) Summary
            var summary = await _summaryService.GetSummaryAsync(conversationId);
            if (!string.IsNullOrWhiteSpace(summary))
            {
                sb.AppendLine("TÓM TẮT NGỮ CẢNH (Conversation Summary):");
                sb.AppendLine(summary);
                sb.AppendLine();
            }

            // 2) Knowledge
            if (knowledge == null || knowledge.Count == 0)
            {
                knowledge = await _knowledgeRepository.SearchByTagsAsync(new[] { "schema", "project" }, take: 1);
            }

            if (knowledge != null && knowledge.Count > 0)
            {
                sb.AppendLine("KIẾN THỨC DỰ ÁN (trích từ tài liệu nội bộ):");
                foreach (var k in knowledge)
                {
                    sb.AppendLine($"--- {k.Title} | tags: {k.Tags} ---");
                    var content = k.Content ?? "";
                    if (content.Length > 4000) content = content.Substring(0, 4000) + "...";
                    sb.AppendLine(content);
                    sb.AppendLine();
                }
            }

            // 3) UI current space context
            if (currentSpaceId != null)
            {
                var space = await _spaceService.GetSpaceByIdAsync(currentSpaceId.Value.ToString());
                if (space != null)
                {
                    sb.AppendLine("NGỮ CẢNH UI HIỆN TẠI (User đang đứng ở Space này):");
                    sb.AppendLine(JsonSerializer.Serialize(new { currentSpace = space }, new JsonSerializerOptions { WriteIndented = true }));
                    sb.AppendLine();
                }
                else
                {
                    sb.AppendLine("NGỮ CẢNH UI HIỆN TẠI: currentSpaceId có nhưng không tìm thấy Space trong DB.");
                    sb.AppendLine();
                }
            }
            else
            {
                sb.AppendLine("NGỮ CẢNH UI HIỆN TẠI: currentSpaceId = null (User chưa chọn Space).");
                sb.AppendLine();
            }

            // 4) Conversation history
            if (msgs.Count > 0)
            {
                sb.AppendLine("LỊCH SỬ HỘI THOẠI (tham khảo):");
                foreach (var m in msgs)
                {
                    var who = (m.Role ?? "").Equals("user", StringComparison.OrdinalIgnoreCase) ? "User" : "Assistant";
                    sb.AppendLine($"{who}: {m.Content}");
                }
                sb.AppendLine();
            }

            // 5) Inject domain data theo intent

            // Tasks
            if (LooksLikeTaskQuery(userMessage))
            {
                var (status, take) = ExtractTaskFilters(userMessage);
                var tasks = await _taskService.GetTasksByUserIdAsync(userId);

                if (!string.IsNullOrWhiteSpace(status))
                {
                    tasks = tasks
                        .Where(t => t.Status != null &&
                                    t.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                var taskResult = tasks
                    .OrderBy(t => t.DueDate ?? DateTime.MaxValue)
                    .Select(t => new
                    {
                        Name = t.Name,
                        Status = t.Status,
                        DueDate = t.DueDate?.ToString("dd/MM/yyyy")
                    })
                    .Take(take)
                    .ToList();

                sb.AppendLine("DỮ LIỆU TASKS (nội bộ hệ thống):");
                sb.AppendLine(JsonSerializer.Serialize(new { tasks = taskResult }, new JsonSerializerOptions { WriteIndented = true }));
                sb.AppendLine();
            }

            // Teams
            if (LooksLikeTeamQuery(userMessage))
            {
                var teams = await _teamService.GetTeamsByUserIdAsync(userId);
                sb.AppendLine("DỮ LIỆU TEAMS CỦA USER (nội bộ hệ thống):");
                sb.AppendLine(JsonSerializer.Serialize(new { teams }, new JsonSerializerOptions { WriteIndented = true }));
                sb.AppendLine();
            }

            // Spaces:
            // - Nếu đang có currentSpaceId và user hỏi "space này" thì không cần bơm all spaces
            // - Nếu currentSpaceId == null hoặc user hỏi kiểu "tất cả spaces" thì bơm
            if (LooksLikeSpaceQuery(userMessage) && (currentSpaceId == null || AsksAllScope(userMessage)))
            {
                var spaces = await _spaceService.GetSpacesByUserIdAsync(userId);
                sb.AppendLine("DỮ LIỆU SPACES CỦA USER (nội bộ hệ thống):");
                sb.AppendLine(JsonSerializer.Serialize(new { spaces }, new JsonSerializerOptions { WriteIndented = true }));
                sb.AppendLine();
            }

            // Lists (ưu tiên currentSpace)
            if (LooksLikeListQuery(userMessage))
            {
                if (currentSpaceId != null && !AsksAllScope(userMessage))
                {
                    var lists = await _listService.GetListsBySpaceIdAsync(currentSpaceId.Value.ToString());
                    sb.AppendLine("DỮ LIỆU LISTS TRONG CURRENT SPACE (nội bộ hệ thống):");
                    sb.AppendLine(JsonSerializer.Serialize(new { lists }, new JsonSerializerOptions { WriteIndented = true }));
                    sb.AppendLine();
                }
                else
                {
                    var lists = await _listService.GetListsByUserIdAsync(userId);
                    sb.AppendLine("DỮ LIỆU LISTS CỦA USER (nội bộ hệ thống):");
                    sb.AppendLine(JsonSerializer.Serialize(new { lists }, new JsonSerializerOptions { WriteIndented = true }));
                    sb.AppendLine();
                }
            }

            // 6) Final user question
            sb.AppendLine("NGƯỜI DÙNG VỪA HỎI:");
            sb.AppendLine(userMessage);
            sb.AppendLine();
            sb.AppendLine("Hãy trả lời:");

            return sb.ToString();
        }

        // ---------- Intent helpers ----------

        private static bool LooksLikeTaskQuery(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;
            var t = text.ToLowerInvariant();

            return t.Contains("task")
                || t.Contains("công việc")
                || t.Contains("todo")
                || t.Contains("doing")
                || t.Contains("done")
                || t.Contains("cần làm")
                || t.Contains("đang làm")
                || t.Contains("hoàn thành");
        }

        private static (string? status, int take) ExtractTaskFilters(string text)
        {
            string? status = null;
            int take = 15;

            var t = (text ?? "").ToLowerInvariant();

            if (Regex.IsMatch(t, @"\btodo\b") || t.Contains("cần làm") || t.Contains("chưa làm"))
                status = "Todo";
            else if (Regex.IsMatch(t, @"\bdoing\b") || t.Contains("đang làm"))
                status = "Doing";
            else if (Regex.IsMatch(t, @"\bdone\b") || t.Contains("hoàn thành") || t.Contains("xong"))
                status = "Done";

            var m = Regex.Match(t, @"\b(\d{1,2})\b");
            if (m.Success && int.TryParse(m.Groups[1].Value, out var n))
            {
                if (n >= 1 && n <= 30) take = n;
            }

            return (status, take);
        }

        private static bool LooksLikeTeamQuery(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;
            var t = text.ToLowerInvariant();
            return t.Contains("team") || t.Contains("nhóm") || t.Contains("đội");
        }

        private static bool LooksLikeSpaceQuery(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;
            var t = text.ToLowerInvariant();
            return t.Contains("space") || t.Contains("không gian") || t.Contains("workspace") || t.Contains("dự án");
        }

        private static bool LooksLikeListQuery(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;
            var t = text.ToLowerInvariant();
            return t.Contains("list") || t.Contains("danh sách");
        }

        // user hỏi kiểu “tất cả”, “toàn bộ”, “all”
        private static bool AsksAllScope(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;
            var t = text.ToLowerInvariant();
            return t.Contains("tất cả") || t.Contains("toàn bộ") || Regex.IsMatch(t, @"\ball\b");
        }

        public async Task DeleteConversationAsync(Guid conversationId, string userId)
        {
            // kiểm tra conversation thuộc về user
            var conv = await _chatRepository.GetConversationByIdAsync(conversationId, userId);
            if (conv == null) throw new KeyNotFoundException("Conversation not found");

            await _chatRepository.DeleteConversationAsync(conversationId, userId);
        }

    }
}
