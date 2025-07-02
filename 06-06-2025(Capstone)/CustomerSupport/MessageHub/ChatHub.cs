using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace CustomerSupport.MessageHub;

[Authorize]
public class ChatHub : Hub
{
    //     private readonly ConnectedUserHandler _connectedUserHandler;

    //     public ChatHub(ConnectedUserHandler connectedUserHandler)
    //     {
    //         _connectedUserHandler = connectedUserHandler;
    //     }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.Identity?.Name;

        Console.WriteLine(userId);

        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            Console.WriteLine($"User {userId} added to group.");
        }
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.User?.Identity?.Name;
        Console.WriteLine(userId);

        // if (!string.IsNullOrEmpty(userId))
        //     _connectedUserHandler.RemoveConnection(userId, Context.ConnectionId);

        return base.OnDisconnectedAsync(exception);
    }

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
