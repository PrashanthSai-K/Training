using System;
using BankingApplication.Models;
using Microsoft.VisualBasic;

namespace BankingApplication.Services;

public class FaqKnowledgeService
{
  private static List<Faq> _faqs = new List<Faq>
    {
        new ()
        {
            Question = "Hi, hello",
            Answer = "Welcome How may I assist you."
        },
        new ()
        {
            Question = "how do i check my account balance",
            Answer = "You can check your account balance by using the 'Balance' API endpoint with your Account ID."
        },
        new ()
        {
            Question = "how do i create a new account",
            Answer = "You can create a new bank account by providing your name, email, and initial balance using the 'CreateAccount' API endpoint."
        },
        new ()
        {
            Question = "how do i withdraw money",
            Answer = "Withdrawals can be made by specifying the amount and your account details using the 'Withdraw' API endpoint."
        },
        new() {
            Question = "how do i deposit money",
            Answer = "You can deposit money into your account by providing deposit details like amount and account ID via the 'Deposit' API endpoint."
        },
        new ()
        {
            Question = "how do i transfer money to another account",
            Answer = "Money transfers between accounts are handled through the 'Transfer' API endpoint, where you specify sender and receiver account information along with the amount."
        },
        new ()
        {
            Question = "what details are required to create an account",
            Answer = "To create an account, you need to provide your Name, Email, and initial Balance."
        },
        new ()
        {
            Question = "can i withdraw more money than my account balance",
            Answer = "Withdrawals are subject to your current account balance. If you try to withdraw more than available, the transaction will be declined."
        },
        new ()
        {
            Question = "how long does a transfer take",
            Answer = "Transfers within the bank are typically instant, but processing times can vary depending on the traffic"
        }
    };

  public static List<Faq> GetMatchingQuestions(string promt)
  {
    if (string.IsNullOrWhiteSpace(promt))
      return new List<Faq>();

    promt = promt?.Trim().ToLower() ?? "";
    var promtWords = promt.Split(' ', StringSplitOptions.RemoveEmptyEntries);

    var fasq = _faqs.FindAll(faq =>
    {
      var question = faq.Question?.ToLower() ?? "";
      return promtWords.Any(word => question.Contains(word));
    });

    return fasq;
  }

}
