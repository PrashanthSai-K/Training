using System;

namespace CustomerSupport.Models.QueryParams;

public class ChatMessageQueryParams : PaginationParams
{
    public string? UserId { get; set; }
    public string? Message { get; set; }
    public DateRange? Date { get; set; }
}
