using System.ComponentModel.DataAnnotations;
using UmtInventoryBackend.Enums;

namespace UmtInventoryBackend.Models;

public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public string Surname { get; set; }
    public string Email { get; set; }
    
    public string? Password { get; set; }
    
    public UserRole Role { get; set; }
    
    [Phone]
    public string Phone { get; set; }
    
    public int WorkspaceID { get; set; } // New property
}
