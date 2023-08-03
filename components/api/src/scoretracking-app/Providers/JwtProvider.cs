using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ScoreTracking.App.Interfaces.Providers;
using ScoreTracking.App.Models;
using ScoreTracking.App.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.App.Providers
{
    public sealed class JwtProvider : IJwtProvider
    {
        private readonly JwtOptions _options;

        public JwtProvider(IOptions<JwtOptions> jwtOptions)
        {
            _options = jwtOptions.Value;
        }

        public string Generate(User user)
        {
            IEnumerable<Claim> claims = new List<Claim>()
           {
               new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
               new Claim(JwtRegisteredClaimNames.Email, user.Email)
           };

        var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _options.Issuer,
                _options.Audience,
                claims,
                null,
                DateTime.UtcNow.AddHours(5),
                signingCredentials
                );

            string tokenValue = new JwtSecurityTokenHandler().WriteToken(token); 
            return tokenValue;
        }
    }
}
