using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarterKit.Data;
using StarterKit.Models;
using StarterKit.Services;

namespace StarterKit.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(AppDbContext context, JwtService jwtService) : ControllerBase
    {
        private readonly AppDbContext _context = context;
        private readonly JwtService _jwtService = jwtService;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User dto)
        {
            var user = new User
            {
                Username = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User dto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Username == dto.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.PasswordHash, user.PasswordHash))
                return Unauthorized();

            var token = _jwtService.GenerateToken(user);
            return Ok(new { token });
        }
    }

}