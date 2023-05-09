using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UmtInventoryBakend.Data;
using UmtInventoryBakend.Entities;

namespace UmtInventoryBakend.Controllers;
[Route("api/[controller]")]
[ApiController]

public class UserController : Controller
{
    ApplicationDbContext _dbContext;
    public UserController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // GET
    [HttpGet]

    public async Task<ActionResult<IEnumerable<User>>> GetClients()
    {
        if (_dbContext.Users == null)
        {

            return NotFound();

        }
        return await _dbContext.Users.ToListAsync();

    }
    
    [HttpGet]
    [Route("GetUserById")]
    public async Task<ActionResult<IEnumerable<User>>> GetClientbyUsername(int id)
    {

        var clientExist = _dbContext.Users.Where(x => x.Id == id);

        if (clientExist == null)
        {
            return NotFound("User not found!");

        }
        return Ok(clientExist);

    }

}