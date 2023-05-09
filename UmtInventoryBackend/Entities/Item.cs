using System.ComponentModel.DataAnnotations;

namespace UmtInventoryBakend.Entities;

public class Item : IEntity
{
    [Key]
    public int Id { get; set; }
    public double Price { get; set; }
    public double Quantity { get; set; }
    public string Condition { get; set; }
    public string Description { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Status { get; set; }

}