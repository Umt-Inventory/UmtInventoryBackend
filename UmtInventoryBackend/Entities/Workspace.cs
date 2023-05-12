using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UmtInventoryBackend.Entities;

public class Workspace : IEntity
{
    [Key] 
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string Type { get; set; }
    
    public int Building { get; set; }
    
    public ICollection<Item> Items { get; set; } // A Workspace can have many Items
    
 
}