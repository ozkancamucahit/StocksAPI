using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Account
{
    public sealed class LoginDTO
    {
        [Required]
        [Length(4, 100)]
        public string UserName { get; set; } = String.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Length(8, 100)]
        public string Password { get; set; } = String.Empty;
    }
}
