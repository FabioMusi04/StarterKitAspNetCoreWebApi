using System.ComponentModel.DataAnnotations;
using static StarterKit.Enums.Enum;

namespace StarterKit.Models
{
    public class UploadFile
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "File name is required.")]
        [StringLength(100, ErrorMessage = "File name cannot be longer than 100 characters.")]
        [MinLength(3, ErrorMessage = "File name must be at least 1 character long.")]
        public string FileName { get; set; } = string.Empty;

        [Required(ErrorMessage = "File path is required.")]
        [StringLength(255, ErrorMessage = "File path cannot be longer than 255 characters.")]
        [MinLength(3, ErrorMessage = "File path must be at least 1 character long.")]
        public string FilePath { get; set; } = string.Empty;

        [Required(ErrorMessage = "Content type is required.")]
        [StringLength(100, ErrorMessage = "Content type cannot be longer than 100 characters.")]
        [MinLength(3, ErrorMessage = "Content type must be at least 1 character long.")]
        public string ContentType { get; set; } = string.Empty;

        [Required]
        [StringLength(50, ErrorMessage = "File format cannot be longer than 50 characters.")]
        public FormatEnum Format { get; set; } = FormatEnum.Png;

        [Required]
        [Range(0, long.MaxValue, ErrorMessage = "Size must be a non-negative number.")]
        public long Size { get; set; } = 0;

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [DataType(DataType.DateTime)]
        public DateTime? UpdatedAt { get; set; } = null;
    }
}
