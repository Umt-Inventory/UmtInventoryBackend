using System.ComponentModel.DataAnnotations;
using UmtInventoryBackend.Entities;
using UmtInventoryBackend.Enums;

namespace UmtInventoryBackend.Models;

public class AddEditItemDto: IEntity
{
    public int Id { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }
    public Condition Condition { get; set; }
    public string Description { get; set; }
    public string Name { get; set; }
    
    public UserType Type { get; set; }
    
    public int WorkspaceId { get; set; } // New foreign key
}