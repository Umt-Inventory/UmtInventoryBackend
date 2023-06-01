using System.Collections.Generic;
using UmtInventoryBackend.Enums;

namespace UmtInventoryBackend.Models;

public class PaginatedItems<T>
{
    public IEnumerable<T> Items { get; set; }
    public int TotalItems { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public UserType? FilterUserType { get; set; }
    
    public string? SearchString { get; set; }
}