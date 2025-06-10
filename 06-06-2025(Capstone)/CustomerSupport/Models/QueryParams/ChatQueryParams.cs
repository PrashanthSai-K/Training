using System;

namespace CustomerSupport.Models.QueryParams;

public class ChatQueryParams : PaginationParams
{
    public int? AgentId { get; set; }
    public int? CustomerId { get; set; }
    public string? Status { get; set; }
    public DateRange? ChatCreatedDate { get; set; }
}
