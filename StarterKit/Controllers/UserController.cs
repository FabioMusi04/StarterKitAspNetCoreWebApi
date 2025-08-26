using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarterKit.Data;
using StarterKit.Models;
using StarterKit.Services;

namespace StarterKit.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(AppDbContext context, IUserService userService) : ControllerBase
{
    private readonly AppDbContext _context = context;
    private readonly IUserService _userService = userService;

    [HttpGet("me")]
    [Authorize]
    public ActionResult<UserDtoResponse> GetMe()
    {
        try
        {
            UserDtoResponse? userDto = _userService.GetCurrentUserAsync(User);
            if (userDto == null) return Unauthorized();

            return Ok(userDto);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("me")]
    [Authorize]
    public async Task<IActionResult> UpdateMe([FromBody] UserDtoRequestUpdate dto)
    {
        try
        {
            UserDtoResponse? user = _userService.GetCurrentUserAsync(User);
            if (user == null) return Unauthorized();

            await _userService.UpdateUserAsync(user.Id, dto);

            return Ok(new { message = "Profile updated." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("me")]
    [Authorize]
    public async Task<IActionResult> DeleteMe()
    {
        try
        {
            UserDtoResponse? user = _userService.GetCurrentUserAsync(User);
            if (user == null) return Unauthorized();

            await _userService.DeleteUserAsync(user.Id);

            return Ok(new { message = "Profile deleted." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<UserDtoResponse>>> GetAllUsers()
    {
        var users = await _context.Users.ToListAsync();
        return Ok(users.Select(u => new UserDtoResponse
        {
            Id = u.Id,
            Username = u.Username,
            Role = u.Role
        }));
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserDtoResponse>> GetUser(int id)
    {
        try
        {
            UserDtoResponse? user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound(new { message = "User not found." });

            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDtoRequestUpdate dto)
    {
        try
        {
            await _userService.UpdateUserAsync(id, dto);

            return Ok(new
            {
                message = "User updated.",
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            await _userService.DeleteUserAsync(id);
            return Ok(new { message = "User deleted." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}