using api.Interfaces;
using api.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace api.Service
{
    public sealed class TokenService : ITokenService
    {
        #region FIELDS
        private readonly IConfiguration configuration;
        private readonly SymmetricSecurityKey key;
        #endregion

        #region CTOR
        public TokenService(IConfiguration configuration)
        {
            this.configuration = configuration;
            key = new(Encoding.UTF8.GetBytes(configuration["JWT:SigninKey"]));
        }
        #endregion


        public string CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            { 
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, user.UserName)
            };

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(3),
                Issuer = configuration["JWT:Issuer"],
                Audience = configuration["JWT:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);

        }



    }
}
