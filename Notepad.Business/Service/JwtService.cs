using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Notepad.Business.Interfaces;
using Notepad.Database.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Notepad.Business.Service
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateSecurityToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("LastPasswordChange", user.LastPasswordChange.ToString("yyyy-MM-dd HH:mm:ss"))
            };

            var token = new JwtSecurityToken(_configuration["JWT:Issuer"], _configuration["JWT:Audience"], claims, expires: DateTime.Now.AddMinutes(60),
                                            signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
