using System.ComponentModel.DataAnnotations;
using UmtInventoryBackend.Enums;

namespace UmtInventoryBackend.Entities;

public class Item : IEntity
{
    public double Price { get; set; }
    public int Quantity { get; set; }
    public Condition Condition { get; set; }

    [MaxLength(100)] public string Description { get; set; }

    [Required] public string Name { get; set; }

    public UserType Type { get; set; }

    /* One-to-many relations */

    public int WorkspaceId { get; set; } // New foreign key

    public virtual Workspace Workspace { get; set; } // Reference to Workspace entity

    [Key] public int Id { get; set; }
}