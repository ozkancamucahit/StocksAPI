using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Account
{
    public sealed class RegisterDTO
    {
        [Required]
        [Length(4, 15)]
        public string UserName { get; set; } = String.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = String.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Length(8, 100)]
        public string Password { get; set; } = String.Empty;


    }
}
