using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UmtInventoryBackend.Data;
using UmtInventoryBackend.Entities;
using UmtInventoryBackend.Enums;

namespace UmtInventoryBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : Controller
{
    private readonly ApplicationDbContext _dbContext;

    public UserController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        if (_dbContext.Users == null) return NotFound();
        return await _dbContext.Users.ToListAsync();
    }

    [HttpGet]
    [Route("GetUserById")]
    public async Task<ActionResult<IEnumerable<User>>> GetClientbyUsername(int id)
    {
        var clientExist = _dbContext.Users.Where(x => x.Id == id);

        if (clientExist == null) return NotFound("User not found!");
        return Ok(clientExist);
    }

    [HttpGet]
    [Route("GetUsersByRole/{role}")]
    public ActionResult<IEnumerable<User>> GetUsersByRole(UserRole role)
    {
        var users = _dbContext.Users.Where(u => u.Role == role).ToList();

        if (users.Count == 0) return NotFound(); // No users found with the specified role

        return Ok(users);
    }


    [HttpPost]
    [Route("AddEditUser")]
    public async Task<ActionResult<User>> PostClients(User user)
    {
        if (user.Id == 0)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, user);
        }

        var userExist = _dbContext.Users.FirstOrDefault(x => x.Id == user.Id);
        if (userExist != null)
        {
            // Update the properties of the existing user
            userExist.Name = user.Name;
            userExist.Email = user.Email;
            userExist.Phone = user.Phone;

            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, userExist);
        }

        return NotFound(); // If the user with the specified Id does not exist
    }

    [HttpDelete]
    [Route("DeleteUser/{id}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        var user = await _dbContext.Users.FindAsync(id);
        if (user == null) return NotFound(); // User with the specified id not found

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();

        return NoContent(); // User successfully deleted
    }
}