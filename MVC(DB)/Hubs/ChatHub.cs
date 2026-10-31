using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace MVC_DB_.Hubs
{
    public class ChatHub : Hub
    {
        // 加入聊天室
        public async Task JoinChat(int friendID)
        {
            await Groups.AddToGroupAsync(
                Context.ConnectionId,
                friendID.ToString()
            );
        }

        // 傳送訊息
        public async Task SendMessage(int friendID, int senderAccountID, string message)
        {
            await Clients.Group(friendID.ToString()).SendAsync(
                "ReceiveMessage",
                friendID,
                senderAccountID,
                message
            );
        }
    }
}