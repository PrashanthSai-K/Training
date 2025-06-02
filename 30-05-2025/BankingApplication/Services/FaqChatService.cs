using System;
using BankingApplication.Interfaces;
using BankingApplication.Models;
using Microsoft.Extensions.AI;
using OllamaSharp;

namespace BankingApplication.Services;

public class FaqChatService : IChatService
{
    private readonly IChatClient _chatClient;
    public FaqChatService()
    {
        _chatClient = new OllamaApiClient(new Uri("http://localhost:11434"), "Banking-Chat:latest");
    }

    public async Task<string> AnswerFaq(string promt)
    {

        List<Faq> faqs = FaqKnowledgeService.GetMatchingQuestions(promt);

        if (faqs == null || faqs.Count == 0)
        {
            return "I’m sorry, I can only help with banking-related topics.";
        }

        var faqText = string.Join("\n", faqs.Select(f => $"Q: {f.Question}\nA: {f.Answer}"));

        var wrappedPrompt =
            $"You are a banking assistant. Use ONLY the following information to answer the user's question.\n\n" +
            $"NOTE : If the User question doesn't feel like related to banking, reject politely that you cant answer" +
            $"Knowledge base:\n{faqText}\n\n" +
            $"User question: {promt}\n\n" +
            $"Answer:";

        var message = "";

        await foreach (ChatResponseUpdate item in _chatClient.GetStreamingResponseAsync(wrappedPrompt))
        {
            message += item.Text;
        }

        return message;
    }
}
