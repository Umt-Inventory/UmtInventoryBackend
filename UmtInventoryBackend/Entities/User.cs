using System.ComponentModel.DataAnnotations;
using UmtInventoryBackend.Enums;

namespace UmtInventoryBackend.Entities;

public class User : IEntity
{
    [Key] 
    public int Id { get; set; }
    public string Name { get; set; }
  
    public string Surname { get; set; }
    public string Email { get; set; }
    
    public string? Password { get; set; }
    public UserRole Role { get; set; }
    public string Phone { get; set; }
    

}