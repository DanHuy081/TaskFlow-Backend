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

        public async Task<Conversation> GetConversationAsync(Guid conversationId, string userId)
        {
            var conversation = await _chatRepository.GetConversationByIdAsync(conversationId, userId);
            if (conversation == null) throw new KeyNotFoundException("Conversation not found");
            return conversation;
        }

        public async Task DeleteConversationAsync(Guid conversationId, string userId)
        {
            var conv = await _chatRepository.GetConversationByIdAsync(conversationId, userId);
            if (conv == null) throw new KeyNotFoundException("Conversation not found");
            await _chatRepository.DeleteConversationAsync(conversationId, userId);
        }

        public async Task<ChatResponseDto> ProcessChatAsync(ChatRequestDto request, string userId)
        {
            // 1. Get or Create Conversation
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

            // 2. Save User Message
            var userMsg = new DBChatMessage
            {
                MessageId = Guid.NewGuid(),
                ConversationId = conversation.ConversationId,
                Role = "user",
                Content = request.Message
            };
            await _chatRepository.AddMessageAsync(userMsg);

            // 3. Search Knowledge Base (RAG)
            var knowledge = await _knowledgeRepository.SearchAsync(request.Message, take: 3);

            // 4. Build Context & Prompt
            var prompt = await BuildPromptAsync(
                conversation.ConversationId,
                conversation,
                request.Message,
                userId,
                knowledge,
                request.CurrentSpaceId,
                request.CurrentTeamId,
                request.CurrentListId
            );

           
            // 5. Call Gemini AI
            var response = await _model.GenerateContent(prompt);
            var aiReply = response?.Text;

            // --- ⚡ BẮT ĐẦU ĐOẠN CODE MỚI: XỬ LÝ ACTION TẠO TASK ---
            // 1. Làm sạch JSON trước (Xóa ```json và ``` và khoảng trắng)
            var jsonClean = (aiReply ?? "").Replace("```json", "").Replace("```", "").Trim();

            // 2. Kiểm tra trên chuỗi ĐÃ LÀM SẠCH (jsonClean) thay vì chuỗi gốc
            if (!string.IsNullOrEmpty(jsonClean) && jsonClean.StartsWith("{") && jsonClean.Contains("create_task"))
            {
                try
                {
                    // Deserialize chuỗi đã làm sạch
                    var actionData = JsonSerializer.Deserialize<AIActionResponse>(jsonClean, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (actionData?.action == "create_task" && actionData.data != null)
                    {
                        if (request.CurrentListId == null)
                        {
                            aiReply = "⚠️ Tôi cần biết bạn muốn tạo task vào List nào. Vui lòng chọn một List cụ thể trên màn hình nhé!";
                        }
                        else
                        {
                            // Map dữ liệu sang TaskCreateDto
                            var newTaskDto = new TaskCreateDto
                            {
                                Name = actionData.data.title ?? "Task mới",
                                
                                ListId = request.CurrentListId.Value.ToString(),
                                Status = "TO DO",

                                // Xử lý ngày tháng an toàn hơn
                                DueDate = !string.IsNullOrEmpty(actionData.data.dueDate) && DateTime.TryParse(actionData.data.dueDate, out var parsedDate)
                                          ? parsedDate
                                          : null,

                                Priority = "Medium"
                            };

                            // Gọi Service tạo Task thật
                            await _taskService.CreateAsync(newTaskDto);

                            // Tạo phản hồi giả lập đè lên JSON cũ
                            aiReply = $"✅ **Đã tạo task thành công!**\n\n" +
                                      $"- **Công việc:** {newTaskDto.Name}\n" +
                                      $"- **Hạn:** {(newTaskDto.DueDate.HasValue ? newTaskDto.DueDate.Value.ToString("dd/MM/yyyy") : "Không có")}\n" +
                                      $"- **Status:** TO DO";
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("AI Action Error: " + ex.ToString());
                    // Nếu lỗi JSON thì kệ, để nó hiện text gốc để mình biết đường sửa tiếp
                }
            }
            // --- ⚡ KẾT THÚC ĐOẠN CODE MỚI ---

            if (string.IsNullOrWhiteSpace(aiReply))
                aiReply = "Xin lỗi, tôi không thể xử lý yêu cầu lúc này.";

            // 6. Save AI Response
            var aiMessage = new DBChatMessage
            {
                MessageId = Guid.NewGuid(),
                ConversationId = conversation.ConversationId,
                Role = "model",
                Content = aiReply
            };
            await _chatRepository.AddMessageAsync(aiMessage);

            // 7. Update Summary & Title
            await _summaryService.UpdateSummaryIfNeededAsync(conversation.ConversationId);

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

        // ================= HELPER METHODS =================

        private async Task<string> BuildPromptAsync(
            Guid conversationId,
            Conversation conversation,
            string userMessage,
            string userId,
            List<KnowledgeChunk> knowledge,
            Guid? currentSpaceId,
            Guid? currentTeamId,
            Guid? currentListId)
        {
            var sb = new StringBuilder();

            // --- 1. SYSTEM PROMPT ---
            sb.AppendLine("SYSTEM INSTRUCTIONS:");
            sb.AppendLine("Bạn là TaskFlow AI - Chuyên gia tư vấn quản lý dự án & hiệu suất.");
            sb.AppendLine();
            sb.AppendLine($"THỜI GIAN HỆ THỐNG HIỆN TẠI: {DateTime.Now:yyyy-MM-dd HH:mm:ss} (Thứ {DateTime.Now.DayOfWeek})");
            sb.AppendLine("PHÂN LOẠI CÂU TRẢ LỜI:");
            sb.AppendLine("1. KHI USER HỎI VỀ DỮ LIỆU (VD: 'Tôi có task nào?', 'Ai đang làm task A?'):");
            sb.AppendLine("   - BẮT BUỘC trả lời dựa trên [CONTEXT DATA]. Nếu không thấy trong Context, bảo không tìm thấy.");
            sb.AppendLine("2. KHI USER HỎI GỢI Ý / TƯ VẤN (VD: 'Nên thêm task gì?', 'Lập kế hoạch cho team Marketing'):");
            sb.AppendLine("   - Hãy đóng vai trò cố vấn chuyên môn. Sử dụng kiến thức rộng của bạn về quản lý dự án.");
            sb.AppendLine("   - Phân tích [CONTEXT DATA] (Tên Team, Mô tả dự án, Task hiện tại) để đưa ra gợi ý sát thực tế nhất.");
            sb.AppendLine("   - Đề xuất các bước đi cụ thể, quy trình chuẩn (Agile/Scrum) phù hợp với tên Team/Space.");
            sb.AppendLine();
            sb.AppendLine("PHONG CÁCH TRẢ LỜI:");
            sb.AppendLine("- Thân thiện, chuyên nghiệp, dùng Emoji hợp lý.");
            sb.AppendLine("- Sử dụng Markdown (Bold, List) để trình bày đẹp mắt.");
            sb.AppendLine();
            sb.AppendLine("CHẾ ĐỘ RA LỆNH (ACTION MODE):");
            sb.AppendLine("Nếu người dùng yêu cầu TẠO TASK (VD: 'Tạo task A', 'Thêm công việc B', 'Giao task C cho D'), BẠN KHÔNG ĐƯỢC TRẢ LỜI BẰNG LỜI.");
            sb.AppendLine("Thay vào đó, hãy trả về DUY NHẤT một chuỗi JSON (không markdown, không giải thích) theo định dạng sau:");
            sb.AppendLine("{");
            sb.AppendLine("  \"action\": \"create_task\",");
            sb.AppendLine("  \"data\": {");
            sb.AppendLine("    \"title\": \"<Tên task trích xuất được>\",");
            sb.AppendLine("    \"description\": \"<Mô tả chi tiết nếu có>\",");
            sb.AppendLine("    \"dueDate\": \"<Ngày hết hạn định dạng yyyy-MM-dd (Nếu user nói 'ngày mai' hãy tự tính ra ngày) hoặc null>\",");
            sb.AppendLine("    \"assigneeName\": \"<Tên người được giao (nếu có) hoặc null>\"");
            sb.AppendLine("  }");
            sb.AppendLine("}");
            sb.AppendLine("Lưu ý: Nếu không tìm thấy thông tin nào (như ngày, người giao), hãy để null.");
            sb.AppendLine();
            sb.AppendLine("QUY TẮC HIỂN THỊ TASK:");
            sb.AppendLine("- Khi liệt kê danh sách task, BẮT BUỘC dùng định dạng Markdown Link chứa ID để user có thể click vào.");
            sb.AppendLine("- Định dạng: [Tên Task](task://<TaskId>)");
            sb.AppendLine("- Ví dụ: [Làm báo cáo](task://123-abc-xyz) - Trạng thái: Todo");
            sb.AppendLine();

            // --- CONTEXT DATA START ---
            sb.AppendLine("[CONTEXT DATA START]");

            // ---------------------------------------------------------
            // 🧠 LOGIC SUY LUẬN CONTEXT (Hierarchy: List -> Space -> Team)
            // ---------------------------------------------------------
            string resolvedTeamId = currentTeamId?.ToString();

            if (string.IsNullOrEmpty(resolvedTeamId) && currentSpaceId != null)
            {
                var currentSpace = await _spaceService.GetSpaceByIdAsync(currentSpaceId.Value.ToString());
                if (currentSpace != null && !string.IsNullOrEmpty(currentSpace.TeamId))
                {
                    resolvedTeamId = currentSpace.TeamId;
                }
            }

            // 1. UI CONTEXT
            sb.AppendLine("### CURRENT UI CONTEXT (Ngữ cảnh làm việc):");

            if (currentListId != null)
            {
                sb.AppendLine($"- Đang đứng trong List ID: {currentListId} (Hãy tập trung vào danh sách này)");
            }

            if (!string.IsNullOrEmpty(resolvedTeamId))
            {
                var currentTeam = await _teamService.GetByIdAsync(resolvedTeamId);
                if (currentTeam != null)
                {
                    sb.AppendLine($"- TEAM HIỆN TẠI: {currentTeam.Name}");
                    sb.AppendLine($"- MÔ TẢ TEAM: {currentTeam.Description ?? "Chưa có mô tả (Hãy gợi ý user bổ sung mô tả để AI hiểu rõ hơn)"}");
                }
            }
            else
            {
                sb.AppendLine("- User đang không ở trong Team cụ thể nào (Global View).");
            }

            if (currentSpaceId != null)
            {
                var space = await _spaceService.GetSpaceByIdAsync(currentSpaceId.Value.ToString());
                if (space != null)
                {
                    sb.AppendLine($"- PROJECT/SPACE HIỆN TẠI: {space.Name}");
                    sb.AppendLine($"- MÔ TẢ PROJECT: {space.Description ?? "Chưa có mô tả"}");
                }
            }

            // 2. Knowledge Base (Docs)
            if (knowledge != null && knowledge.Any())
            {
                sb.AppendLine("### REFERENCE DOCUMENTS (Tài liệu tham khảo):");
                foreach (var k in knowledge)
                {
                    var content = k.Content?.Length > 500 ? k.Content.Substring(0, 500) + "..." : k.Content;
                    sb.AppendLine($"- {k.Title}: {content}");
                }
            }

            // 3. Data Injection (Tasks) - SMART SCOPE DETECTION
            // Kích hoạt khi hỏi Task HOẶC hỏi Gợi ý
            bool isSuggestionQuery = userMessage.ToLower().Contains("gợi ý") ||
                                     userMessage.ToLower().Contains("thêm") ||
                                     userMessage.ToLower().Contains("kế hoạch") ||
                                     userMessage.ToLower().Contains("ý tưởng");

            if (LooksLikeTaskQuery(userMessage) || isSuggestionQuery)
            {
                var (statusFilter, take) = ExtractTaskFilters(userMessage);
                List<TaskFL> tasksSource = new List<TaskFL>();
                string sourceNote = "";

                // 👇 LOGIC MỚI: PHÁT HIỆN Ý ĐỊNH NGƯỜI DÙNG ĐỂ CHỌN PHẠM VI (SCOPE)
                string msgLower = userMessage.ToLower();
                bool userAskForTeam = msgLower.Contains("team") || msgLower.Contains("nhóm") || msgLower.Contains("tất cả") || msgLower.Contains("toàn bộ");
                bool userAskForSpace = msgLower.Contains("dự án") || msgLower.Contains("project") || msgLower.Contains("space");

                // CASE 1: Nếu User hỏi đích danh "Team" -> Lấy Task cả Team (Bỏ qua List hiện tại)
                if (userAskForTeam && !string.IsNullOrEmpty(resolvedTeamId))
                {
                    tasksSource = (await _taskService.GetTasksByTeamIdAsync(resolvedTeamId, take)).ToList();
                    sourceNote = $"trong toàn bộ Team (theo yêu cầu của bạn)";
                }
                // CASE 2: Nếu User hỏi đích danh "Dự án/Space" -> Lấy theo Space (Tạm thời fallback về Team nếu chưa có hàm Space riêng)
                else if (userAskForSpace && (currentSpaceId != null || !string.IsNullOrEmpty(resolvedTeamId)))
                {
                    // Fallback: Lấy theo Team nhưng filter lại (hoặc gọi hàm Space nếu có)
                    tasksSource = (await _taskService.GetTasksByTeamIdAsync(resolvedTeamId, take)).ToList();
                    sourceNote = "trong Dự án này";
                }
                // CASE 3: Mặc định (Ưu tiên ngữ cảnh hẹp nhất: List -> Team -> Personal)
                else if (currentListId != null)
                {
                    tasksSource = (await _taskService.GetTasksByListIdAsync(currentListId.Value.ToString(), take)).ToList();
                    sourceNote = "trong List hiện tại";
                }
                else if (!string.IsNullOrEmpty(resolvedTeamId))
                {
                    tasksSource = (await _taskService.GetTasksByTeamIdAsync(resolvedTeamId, take)).ToList();
                    sourceNote = "trong Team này";
                }
                else
                {
                    tasksSource = (await _taskService.GetTasksByUserIdAsync(userId)).ToList();
                    sourceNote = "của cá nhân bạn";
                }

                // Lọc theo trạng thái
                if (!string.IsNullOrEmpty(statusFilter))
                {
                    tasksSource = tasksSource.Where(t => t.Status?.Equals(statusFilter, StringComparison.OrdinalIgnoreCase) == true).ToList();
                }

                var leanTasks = tasksSource
                    .OrderByDescending(t => t.DateCreated)
                    .Take(take)
                    .Select(t => new
                    {
                        Id = t.Id, // ID để mở Modal (Lưu ý: Nếu Model bạn dùng Id thì sửa thành t.Id)
                        Task = t.Name,
                        Status = t.Status,
                        Priority = t.Priority,

                        // 👇 Logic nối tên tất cả người làm
                        Assignee = (t.TaskAssignees != null && t.TaskAssignees.Any())
                            ? string.Join(", ", t.TaskAssignees.Select(ta => ta.UserFLs != null ? (ta.UserFLs.FullName ?? ta.UserFLs.Username) : "Unknown"))
                            : "Unassigned",

                        Due = t.DueDate?.ToString("dd/MM/yyyy")
                    });

                sb.AppendLine($"### EXISTING TASKS ({sourceNote} - Dùng để tham khảo tiến độ):");
                sb.AppendLine(JsonSerializer.Serialize(leanTasks));
            }

            // 4. Teams & Members
            if (LooksLikeTeamQuery(userMessage) || isSuggestionQuery)
            {
                var teams = await _teamService.GetTeamsWithMembersByUserIdAsync(userId);

                var leanTeams = teams.Select(t => new
                {
                    TeamName = t.Name,
                    Description = t.Description,
                    IsCurrentTeam = !string.IsNullOrEmpty(resolvedTeamId) && t.TeamId.ToString() == resolvedTeamId,
                    Members = t.TeamMembers?.Select(tm => new
                    {
                        Name = tm.UserFLs?.FullName ?? tm.UserFLs?.Username ?? "Unknown User",
                        Email = tm.UserFLs?.Email,
                        Role = tm.Role
                    }).ToList()
                });

                sb.AppendLine("### USER'S TEAMS (Danh sách thành viên):");
                sb.AppendLine(JsonSerializer.Serialize(leanTeams));
            }

            // 5. Spaces
            if (LooksLikeSpaceQuery(userMessage))
            {
                var spaces = await _spaceService.GetSpacesByUserIdAsync(userId);
                var leanSpaces = spaces.Select(s => new { SpaceName = s.Name, Id = s.SpaceId });
                sb.AppendLine("### USER'S SPACES:");
                sb.AppendLine(JsonSerializer.Serialize(leanSpaces));
            }

            sb.AppendLine("[CONTEXT DATA END]");
            sb.AppendLine();

            // --- HISTORY ---
            var historyMsgs = (conversation.Messages ?? new List<DBChatMessage>())
                                .OrderBy(m => m.DateCreated)
                                .TakeLast(6)
                                .ToList();

            if (historyMsgs.Any())
            {
                sb.AppendLine("CONVERSATION HISTORY:");
                foreach (var msg in historyMsgs)
                {
                    var role = msg.Role == "user" ? "User" : "Assistant";
                    sb.AppendLine($"{role}: {msg.Content}");
                }
                sb.AppendLine();
            }

            sb.AppendLine($"User Question: {userMessage}");
            sb.AppendLine("Assistant Answer:");

            return sb.ToString();
        }

        // ================= INTENT DETECTION HELPERS =================

        private static bool LooksLikeTaskQuery(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;
            var t = text.ToLowerInvariant();
            return t.Contains("task") || t.Contains("công việc") || t.Contains("todo") ||
                   t.Contains("doing") || t.Contains("done") || t.Contains("cần làm") ||
                   t.Contains("đang làm") || t.Contains("hoàn thành") || t.Contains("dự án này");
        }

        private static (string? status, int take) ExtractTaskFilters(string text)
        {
            string? status = null;
            int take = 20;

            var t = (text ?? "").ToLowerInvariant();

            if (Regex.IsMatch(t, @"\btodo\b") || t.Contains("cần làm") || t.Contains("chưa làm"))
                status = "Todo";
            else if (Regex.IsMatch(t, @"\bdoing\b") || t.Contains("đang làm"))
                status = "Doing";
            else if (Regex.IsMatch(t, @"\bdone\b") || t.Contains("hoàn thành") || t.Contains("xong"))
                status = "Done";

            return (status, take);
        }

        private static bool LooksLikeTeamQuery(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;
            var t = text.ToLowerInvariant();
            return t.Contains("team") || t.Contains("nhóm") || t.Contains("đội") || t.Contains("thành viên") || t.Contains("ai");
        }

        private static bool LooksLikeSpaceQuery(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;
            var t = text.ToLowerInvariant();
            return t.Contains("space") || t.Contains("không gian") || t.Contains("workspace") || t.Contains("dự án");
        }
    }
}