using System;
using Microsoft.AspNetCore.SignalR;
using Notify.Models;

namespace Notify.Misc;

public class NotificationHub : Hub
{
    public async Task SendMessage(string message)
    {
        await Clients.All.SendAsync("FileUploaded", message);
    } 
}
