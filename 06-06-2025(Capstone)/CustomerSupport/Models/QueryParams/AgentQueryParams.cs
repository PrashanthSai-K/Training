using System;

namespace CustomerSupport.Models.QueryParams;

public class AgentQueryParams
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
