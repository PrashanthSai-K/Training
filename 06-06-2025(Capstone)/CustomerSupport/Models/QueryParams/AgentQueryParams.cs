using System;

namespace CustomerSupport.Models.QueryParams;

public class AgentQueryParams : PaginationParams
{
    public string? Query { get; set; }
}
