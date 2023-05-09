using System.ComponentModel.DataAnnotations;

namespace UmtInventoryBakend.Entities;

public class Ticket : IEntity
{
    [Key]
    public int Id { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
   
}