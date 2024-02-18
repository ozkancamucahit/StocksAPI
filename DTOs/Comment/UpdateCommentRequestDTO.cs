using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Comment
{
    public sealed class UpdateCommentRequestDTO
    {
        [Required]
        [MaxLength(100)]
        [MinLength(5)]
        public string Title { get;  set; } = String.Empty;

        [Required]
        [MaxLength(300)]
        [MinLength(30)]
        public string Content { get;  set; } = String.Empty;
    }
}