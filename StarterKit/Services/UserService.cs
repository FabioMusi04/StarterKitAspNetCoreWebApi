using Microsoft.EntityFrameworkCore;
using StarterKit.Data;
using StarterKit.Models;
using System.Security.Claims;
using static StarterKit.Enums.Enum;

namespace StarterKit.Services;

public interface IUserService
{
    Task RegisterAsync(UserDtoRequestRegister dto);
    Task<(string Token, UserDtoLoginResponse User)> LoginAsync(string usernameOrEmail, string password);
    UserDtoResponse? GetCurrentUserAsync(ClaimsPrincipal User);
    Task<UserDtoResponse?> GetUserByIdAsync(int userId);
    Task UpdateUserAsync(int userId, UserDtoRequestUpdate dto);
    Task DeleteUserAsync(int userId);
    Task<IEnumerable<UserDtoResponse>> GetAllUsersAsync();
}

public class UserService(AppDbContext context, IJwtService jwtService) : IUserService
{
    private readonly AppDbContext _context = context;
    private readonly IJwtService _jwtService = jwtService;

    public async Task RegisterAsync(UserDtoRequestRegister dto)
    {
        bool userExists = await _context.Users.AnyAsync(u => u.Username == dto.Username || u.Email == dto.Email);
        if (userExists)
            throw new Exception("Username or email already in use.");

        User user = new()
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            CreatedAt = DateTime.UtcNow,
            Status = UserStatusEnum.Active
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<(string Token, UserDtoLoginResponse User)> LoginAsync(string usernameOrEmail, string password)
    {
        User? user = await _context.Users.SingleOrDefaultAsync(x => x.Username == usernameOrEmail || x.Email == usernameOrEmail);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid username or password.");

        user.LastLoginAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        UserDtoLoginResponse userDto = new()
        {
            Id = user.Id,
            Username = user.Username,
            Role = user.Role
        };

        var token = _jwtService.GenerateToken(user);

        return (token, userDto);
    }

    public UserDtoResponse? GetCurrentUserAsync(ClaimsPrincipal User)
    {
        User? user = User.FindFirstValue(ClaimTypes.NameIdentifier) is string userIdStr && int.TryParse(userIdStr, out int userId)
            ? _context.Users.Find(userId)
            : null;

        if (user == null) return null;

        return new UserDtoResponse
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            LastLoginAt = user.LastLoginAt,
            Status = user.Status
        };
    }

    public async Task<UserDtoResponse?> GetUserByIdAsync(int userId)
    {
        if (userId <= 0)
            throw new ArgumentException("Invalid user ID.");

        User? user = await _context.Users.FindAsync(userId);

        return user == null
            ? throw new KeyNotFoundException("User not found.")
            : new UserDtoResponse
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                LastLoginAt = user.LastLoginAt,
                Status = user.Status
            };
    }

    public async Task UpdateUserAsync(int userId, UserDtoRequestUpdate dto)
    {
        if (userId <= 0)
            throw new ArgumentException("Invalid user ID.");

        if (dto == null)
            throw new ArgumentNullException(nameof(dto), "User update data cannot be null.");

        User? user = await _context.Users.FindAsync(userId) ?? throw new KeyNotFoundException("User not found.");
        user.Username = dto.Username ?? user.Username;
        user.Email = dto.Email ?? user.Email;
        user.Role = dto.Role ?? user.Role;
        user.UpdatedAt = DateTime.UtcNow;
        user.IsPrivate = dto.IsPrivate ?? user.IsPrivate;

        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(int userId)
    {
        if (userId <= 0)
            throw new ArgumentException("Invalid user ID.");

        User? user = await _context.Users.FindAsync(userId) ?? throw new KeyNotFoundException("User not found.");
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public Task<IEnumerable<UserDtoResponse>> GetAllUsersAsync()
    {
        throw new NotImplementedException();
    }
}