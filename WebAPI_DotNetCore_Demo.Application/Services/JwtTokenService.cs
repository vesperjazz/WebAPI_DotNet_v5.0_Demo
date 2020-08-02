using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using WebAPI_DotNetCore_Demo.Application.Services.Interfaces;

namespace WebAPI_DotNetCore_Demo.Application.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        public string GetAccessToken(Guid UserID, string userName, IEnumerable<string> roles,
            string issuer, string audience, string secretKey, DateTime expirationDate)
        {
            // @TODO Add validation of parameters, e.g expirationDate must be larger than current date time.

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, UserID.ToString()),
                new Claim(ClaimTypes.Name, userName)
            };

            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            // Install-Package System.IdentityModel.Tokens.Jwt
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expirationDate,
                signingCredentials: signingCredentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return jwt;
        }
    }
}
