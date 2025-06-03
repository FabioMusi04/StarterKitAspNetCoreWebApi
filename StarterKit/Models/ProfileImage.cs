using System.ComponentModel.DataAnnotations;
using static StarterKit.Enums.Enum;

namespace StarterKit.Models
{
    public class ProfileImage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.ImageUrl, ErrorMessage = "Invalid profile picture URL format.")]
        [StringLength(200, ErrorMessage = "Profile picture URL cannot be longer than 200 characters.")]
        public string Url { get; set; } = "https://placehold.co/400";

        [Required]
        [StringLength(50, ErrorMessage = "Image format cannot be longer than 50 characters.")]
        public ImageFormatEnum Format { get; set; } = ImageFormatEnum.Png;

        [Required]
        [Range(1, 1000000, ErrorMessage = "Image size must be between 1 and 1,000,000 bytes.")]
        public int Size { get; set; } = 0;

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [DataType(DataType.DateTime)]
        public DateTime? UpdatedAt { get; set; } = null;
    }
}
