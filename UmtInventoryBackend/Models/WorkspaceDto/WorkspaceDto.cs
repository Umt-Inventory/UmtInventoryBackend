using UmtInventoryBackend.Enums;

namespace UmtInventoryBackend.Models.WorkspaceDto;

public class WorkspaceDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public WorkspaceTypes Type { get; set; }
    public Buildings Building { get; set; }
}
