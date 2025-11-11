using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ExpenseTracker.Interfaces;
using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;

namespace ExpenseTracker.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
        {
            _config = config;
            var key = _config["JWT:SigningKey"] ?? "key";
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        }
        public string CreateToken(AppUser user)
        {
            if (user.Email == null || user.UserName == null)
                return "Credntials Required";

            var claims = new List<Claim>
              {
                new(JwtRegisteredClaimNames.Sub,user.Id),
                new(ClaimTypes.NameIdentifier, user.Id), // 👈 add this line
                new(JwtRegisteredClaimNames.Email,user.Email),
                new(JwtRegisteredClaimNames.GivenName,user.UserName)
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(3),
                SigningCredentials = creds,
                Issuer = _config["JWT:Issuer"],
                Audience = _config["JWT:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}