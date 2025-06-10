using System;

namespace CustomerSupport.Models.QueryParams;

public class CustomerQueryParams : PaginationParams
{
    public string? Name { get; set; }
    public string? Email { get; set; }

}
