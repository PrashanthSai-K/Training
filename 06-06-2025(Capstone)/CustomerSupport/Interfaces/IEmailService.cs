using System;

namespace CustomerSupport.Interfaces;

public interface IEmailService
{
    public bool SendEmail(string email, string token);
}
