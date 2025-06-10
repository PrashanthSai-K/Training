using System;

namespace CustomerSupport.Interfaces;

public interface IOtherContextFunctions
{
    public Task<bool> IsUserInChat(int chatId, string email);
}
