using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UmtInventoryBackend.Data;
using UmtInventoryBackend.Entities;
using UmtInventoryBackend.Enums;
using UmtInventoryBackend.Models;
using UmtInventoryBackend.Services;

namespace UmtInventoryBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    private readonly HashingService _hashingService;
    public UserController(ApplicationDbContext dbContext , HashingService hashingService )
    {
        _dbContext = dbContext;
        _hashingService = hashingService;

    }

    [HttpGet]
    public ActionResult<PaginatedUsers<UserDto>> GetUsers(int page = 1, int pageSize = 10, UserRole filterUserRole = UserRole.IT)
    {
        var query = _dbContext.Users.AsQueryable();

        if (filterUserRole != UserRole.IT)
        {
            query = query.Where(u => u.Role == filterUserRole);
        }

        var users = query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
        
        // Transform users to UserDto
        var userDtos = users.Select(u => new UserDto
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
            Role = u.Role,
            Phone = u.Phone,
            WorkspaceID = u.WorkspaceID
        }).ToList();

        var totalUsers = query.Count();

        var paginatedUsers = new PaginatedUsers<UserDto>
        {
            Users = userDtos,
            TotalUsers = totalUsers,
            Page = page,
            PageSize = pageSize,
            FilterUserType = filterUserRole
        };

        return Ok(paginatedUsers);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUserById(int id)
    {
        var user = await _dbContext.Users.FindAsync(id);

        if (user == null) return NotFound("User not found!");
        return Ok(user);
    }

    [HttpGet("GetUsersByRole/{role}")]
    public async Task<ActionResult<IEnumerable<User>>> GetUsersByRole(UserRole role)
    {
        var users = await _dbContext.Users.Where(u => u.Role == role).ToListAsync();

        if (users.Count == 0) return NotFound("No users found with the specified role"); 

        return Ok(users);
    }



    [HttpPost("AddEditUser")]
    public async Task<ActionResult<UserDto>> AddEditUser(UserCreateUpdateDto userDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        User user;

        if (userDto.Id == 0)
        {
            // Creating a new user
            user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                Surname = userDto.Surname,
                Password = _hashingService.HashPassword(userDto.Password),
                Role = userDto.Role,
                Phone = userDto.Phone,
                WorkspaceID = userDto.WorkspaceID
            };

            _dbContext.Users.Add(user);
        }
        else
        {
            // Updating an existing user
            user = await _dbContext.Users.FindAsync(userDto.Id);

            if (user == null)
            {
                return NotFound("User not Found!"); // If the user with the specified Id does not exist
            }

            user.Name = userDto.Name;
            user.Email = userDto.Email;
            user.Password = userDto.Password != null ? _hashingService.HashPassword(userDto.Password) : user.Password;
            user.Phone = userDto.Phone;
            user.WorkspaceID = userDto.WorkspaceID;
        }

        await _dbContext.SaveChangesAsync();

        var returnUserDto = new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            Phone = user.Phone,
            WorkspaceID = user.WorkspaceID
        };

        return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, returnUserDto);
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