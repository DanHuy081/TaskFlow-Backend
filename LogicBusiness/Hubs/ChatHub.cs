using Microsoft.AspNetCore.SignalR;
using Mscc.GenerativeAI;
using System.Text.RegularExpressions;

namespace TaskFlowBE.Hubs
{
    public class ChatHub : Hub
    {
        // Frontend gọi hàm này khi user vào trang dự án
        public async Task JoinTeamRoom(string teamId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, teamId);
        }

        public async Task LeaveTeamRoom(string teamId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, teamId);
        }
    }
}
