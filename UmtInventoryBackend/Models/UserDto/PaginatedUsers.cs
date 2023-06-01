using UmtInventoryBackend.Enums;

namespace UmtInventoryBackend.Models;

public class PaginatedUsers<T>
{
    public IEnumerable<T> Users { get; set; }
    public int TotalUsers { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public UserRole FilterUserType { get; set; }
    
    public string? SearchString { get; set; }
}