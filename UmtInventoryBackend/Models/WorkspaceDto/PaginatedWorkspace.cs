using UmtInventoryBackend.Enums;

namespace UmtInventoryBackend.Models.WorkspaceDto;

public class PaginatedWorkspace<T>
{
    public IEnumerable<T> Workspace { get; set; }
    public int TotalWorkspaces { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public Buildings? FilterBuilding { get; set; }
}
