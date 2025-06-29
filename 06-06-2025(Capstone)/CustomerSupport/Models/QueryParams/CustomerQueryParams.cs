using System;

namespace CustomerSupport.Models.QueryParams;

public class CustomerQueryParams : PaginationParams
{
    public string? Query { get; set; }

}
