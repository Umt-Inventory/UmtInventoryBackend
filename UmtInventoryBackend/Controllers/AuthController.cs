using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UmtInventoryBackend.Data;
using UmtInventoryBackend.Models;
using UmtInventoryBackend.Services;

namespace UmtInventoryBackend.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    private readonly TokenService _tokenService;
    private readonly HashingService _hashingService;
    public AuthController(ApplicationDbContext dbContext, TokenService tokenService, HashingService hashingService)
    {
        _dbContext = dbContext;
        _tokenService = tokenService;
        _hashingService = hashingService;
    }
    [HttpPost]
    [Route("Login")]
    [AllowAnonymous]
    public async Task<ActionResult<TokenResponse>> Login(UserLoginDto userLoginDto)
    {
        var user = await _dbContext.Users.Where(u => u.Email == userLoginDto.Email).FirstOrDefaultAsync();

        if (user == null)
        {
            return NotFound("User not found!"); // User with the specified email not found
        }

        if(!_hashingService.CheckPassword(user.Password, userLoginDto.Password))
        {
            return Unauthorized("Invalid credentials"); // Incorrect password
        }

        // If user is found and password is correct, generate JWT token
        var token = new TokenResponse
        {

            Token = _tokenService.GenerateToken(user)
        };
  

        return Ok(token);
    }


}