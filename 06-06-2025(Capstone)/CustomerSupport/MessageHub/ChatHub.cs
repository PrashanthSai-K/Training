using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace CustomerSupport.MessageHub;

[Authorize]
public class ChatHub : Hub
{
    public async Task SendMessage(string chatId, string userId, string message)
    {
        await Clients.Group(chatId).SendAsync("ReceiveMessage", chatId, userId, message);
    }
    public async Task JoinGroup(int chatId)
    {
        try
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
