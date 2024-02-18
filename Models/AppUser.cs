using Microsoft.AspNetCore.Identity;

namespace api.Models
{
    public sealed class AppUser : IdentityUser
    {
        public List<Portfolio> Portfolios { get; set; } = new();
    }
}
