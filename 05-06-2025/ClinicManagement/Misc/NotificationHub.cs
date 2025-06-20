using System;
using Microsoft.AspNetCore.SignalR;

namespace ClinicManagement.Misc;

public class NotificationHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}
