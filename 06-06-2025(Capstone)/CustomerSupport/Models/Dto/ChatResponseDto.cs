using CustomerSupport.Models;

namespace CustomerSupport.Models.Dto;

public class ChatResponseDto
{
    public int Id { get; set; }
    public string IssueName { get; set; } = string.Empty;
    public string IssueDescription { get; set; } = string.Empty;
    public int AgentId { get; set; }
    public int CustomerId { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Status { get; set; } = string.Empty;

    public AgentResponseDto? Agent { get; set; }
    public CustomerResponseDto? Customer { get; set; }

}