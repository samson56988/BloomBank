using CustomerAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CustomerAPI.Helpers
{
    public class UserTokenGenerator
    {
        private readonly IConfiguration _configuration;

        public UserTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(UserTokenDetails userTokenDetails)
        {
            var claims = new[]
            {
                new Claim("UserId", userTokenDetails.UserId),
                new Claim("Email", userTokenDetails.Email),
                new Claim("AccountNo", userTokenDetails.AccountNo)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["AuthSettings:Issuer"],
                audience: _configuration["AuthSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            
            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenAsString;
        }
    }
}
