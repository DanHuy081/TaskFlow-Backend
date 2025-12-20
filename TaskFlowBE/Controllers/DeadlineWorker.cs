using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using LogicBusiness.UseCase; // Namespace chứa INotificationService
using LogicBusiness.Repository; // Namespace chứa ITaskRepository
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class DeadlineWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public DeadlineWorker(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Vòng lặp chạy mãi mãi cho đến khi tắt ứng dụng
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CheckDeadlinesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi Worker: {ex.Message}");
            }

            // Nghỉ 1 tiếng (hoặc 30 phút) rồi mới quét tiếp
            // TimeSpan.FromHours(1) hoặc TimeSpan.FromMinutes(30)
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }

    private async Task CheckDeadlinesAsync()
    {
        // Tạo một scope mới để lấy được Repository và Service
        using (var scope = _scopeFactory.CreateScope())
        {
            var taskRepo = scope.ServiceProvider.GetRequiredService<ITaskRepository>();
            var notiService = scope.ServiceProvider.GetRequiredService<INotificationService>();

            // Logic: Quét các task sẽ hết hạn trong 24h tới
            // Tuy nhiên để tránh Spam (báo đi báo lại mỗi tiếng), 
            // cách đơn giản nhất là chỉ quét các task hết hạn trong "khung giờ hiện tại + 1 tiếng"

            var now = DateTime.Now;
            var nextHour = now.AddHours(1);

            // Tìm task có deadline rơi vào khoảng [Bây giờ -> 1 tiếng nữa]
            var tasksDueSoon = await taskRepo.GetTasksDueInIntervalAsync(now, nextHour);

            foreach (var task in tasksDueSoon)
            {
                // Task này có ai làm không?
                if (task.TaskAssignees != null)
                {
                    foreach (var assignee in task.TaskAssignees)
                    {
                        // Convert UserId from string to Guid
                        if (Guid.TryParse(assignee.UserId, out Guid userId))
                        {
                            // Gửi thông báo cho từng người
                            string title = "⚠️ Nhắc nhở hạn chót!";
                            string message = $"Công việc '{task.Name}' sắp đến hạn vào lúc {task.DateClosed:HH:mm}. Hãy hoàn thành sớm!";

                            // Gọi hàm tạo thông báo cũ
                            await notiService.CreateNotification(userId, title, message);
                        }
                        else
                        {
                            Console.WriteLine($"Lỗi: Không thể chuyển đổi UserId '{assignee.UserId}' thành Guid.");
                        }
                    }
                }
            }
        }
    }
}