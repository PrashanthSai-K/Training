using System;

namespace CustomerSupport.Interfaces;

public interface IOtherContextFunctions
{
    public Task<bool> IsUserInChat(int chatId, string email);
    public Task<bool> IsUsernameExists(string username);
}
