using System.Security.Cryptography;
using System.Text;

namespace UmtInventoryBackend.Services;

public class HashingService
{
    public string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
    }

    public bool CheckPassword(string hash, string password)
    {
        return hash == HashPassword(password);
    }
}