using Microsoft.AspNetCore.Mvc;
using StarterKit.Data;
using StarterKit.Models;
using StarterKit.Services;

namespace StarterKit.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(AppDbContext context, IUserService userService) : ControllerBase
{
    private readonly AppDbContext _context = context;
    private readonly IUserService _userService = userService;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserDtoRequestRegister user)
    {
        try
        {
            await _userService.RegisterAsync(user);
            return Ok(new { message = "User registered successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserDtoRequestLogin user)
    {
        try
        {
            var (token, userDto) = await _userService.LoginAsync(user.UsernameOrEmail, user.Password);
            return Ok(new { token, user = userDto });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });

        }
    }
}