using System.ComponentModel.DataAnnotations;

namespace UmtInventoryBakend.Entities;

public class User : IEntity
{
    [Key]
    public int Id { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public int Phone { get; set; }
    public ICollection<Workspace> Workspace { get; set; }
   
}