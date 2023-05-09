using System.ComponentModel.DataAnnotations;

namespace UmtInventoryBakend.Entities;

public class Workspace : IEntity
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public int Building { get; set; }
   
}