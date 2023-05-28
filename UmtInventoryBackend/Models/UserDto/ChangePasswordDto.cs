namespace UmtInventoryBackend.Models;

public class ChangePasswordDto
{
    public int Id { get; set; }
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}