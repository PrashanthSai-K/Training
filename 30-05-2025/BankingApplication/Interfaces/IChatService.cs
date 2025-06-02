using System;

namespace BankingApplication.Interfaces;

public interface IChatService
{
    Task<string> AnswerFaq(string promt);
}   
