using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static StarterKit.Enums.Enum;

namespace StarterKit.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "Username cannot be longer than 100 characters.")]
    [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username can only contain letters, numbers, and underscores.")]
    [MinLength(3, ErrorMessage = "Username must be at least 3 characters long.")]
    public string Username { get; set; } = "";

    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address format.")]
    [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters.")]
    public string Email { get; set; } = "";

    [Required]
    [StringLength(100, ErrorMessage = "Password cannot be longer than 100 characters.")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{6,}$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one number.")]
    public string PasswordHash { get; set; } = "";

    public int? ProfileImageId { get; set; } = null;

    [ForeignKey("ProfileImageId")]
    public virtual UploadFile? ProfileImage { get; set; } = null;

    public UserRoleEnum Role { get; set; } = UserRoleEnum.User;

    [DataType(DataType.DateTime)]
    public DateTime? LastLoginAt { get; set; } = null;

    public UserStatusEnum Status { get; set; } = UserStatusEnum.Inactive;

    [Required]
    public bool IsPrivate { get; set; } = false;

    public bool IsEmailVerified { get; set; } = false;

    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [DataType(DataType.DateTime)]
    public DateTime? UpdatedAt { get; set; } = null;
}

public class UserDtoRequestRegister
{
    [Required(ErrorMessage = "Username is required.")]
    [StringLength(100, ErrorMessage = "Username cannot be longer than 100 characters.")]
    [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username can only contain letters, numbers, and underscores.")]
    [MinLength(3, ErrorMessage = "Username must be at least 3 characters long.")]
    public string Username { get; set; } = "";

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address format.")]
    [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters.")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, ErrorMessage = "Password cannot be longer than 100 characters.")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "";
}

public class UserDtoRequestLogin
{
    [Required(ErrorMessage = "Username or email is required.")]
    [StringLength(100, ErrorMessage = "Username or email cannot be longer than 100 characters.")]
    public string UsernameOrEmail { get; set; } = "";
    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, ErrorMessage = "Password cannot be longer than 100 characters.")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "";
}

public class UserDtoRequestUpdate
{
    [StringLength(100)]
    [RegularExpression(@"^[a-zA-Z0-9_]+$")]
    [MinLength(3)]
    public string? Username { get; set; }

    [EmailAddress]
    [StringLength(100)]
    public string? Email { get; set; }

    public IFormFile? ProfileImage { get; set; }

    public bool? IsPrivate { get; set; }

    public UserRoleEnum? Role { get; set; }
}


public class UserDtoLoginResponse
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public UserRoleEnum Role { get; set; } = UserRoleEnum.User;
}

public class UserDtoResponse
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public UserRoleEnum Role { get; set; } = UserRoleEnum.User;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = null;
    public DateTime? LastLoginAt { get; set; } = null;
    public UserStatusEnum Status { get; set; } = UserStatusEnum.Inactive;
    public bool IsPrivate { get; set; } = false;
    public bool IsEmailVerified { get; set; } = false;
    public UploadFile? ProfileImage { get; set; } = null;

}