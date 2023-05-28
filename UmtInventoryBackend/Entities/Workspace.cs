using System.ComponentModel.DataAnnotations;
using UmtInventoryBackend.Enums;

namespace UmtInventoryBackend.Entities;

public class Workspace : IEntity
{
    public string Name { get; set; }

    public WorkspaceTypes Type { get; set; }

    public Buildings Building { get; set; }

    public ICollection<Item> Items { get; set; } // A Workspace can have many Items

    [Key] public int Id { get; set; }
}