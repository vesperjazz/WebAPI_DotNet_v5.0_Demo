using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using WebAPI_DotNetCore_Demo.Application.Services;
using Xunit;

namespace WebAPI_DotNetCore_Demo.Application.Tests
{
    public class JwtTokenServiceTests
    {
        private readonly JwtTokenService _jwtTokenServiceSUT;
        public JwtTokenServiceTests()
        {
            _jwtTokenServiceSUT = new JwtTokenService();
        }

        public static IEnumerable<object[]> ValidTokenData => new List<object[]>
        {
            new object[] { Guid.NewGuid(), "aragorn.elessar", new List<string> { "Admin" }, "issuer", "audience", "GE0an1wtNvpuG4Czzd7L", DateTime.Now.Date.AddDays(1) },
            new object[] { Guid.NewGuid(), "arwen.undomiel", new List<string> { "User" }, "", "audience", "fDndUOSNixFHWCKUFirn", DateTime.Now.Date.AddMinutes(15) },
            new object[] { Guid.NewGuid(), "gandalf.greyhame", new List<string> { "Admin", "User" }, "", "", "0krXcp50wLyi6JCSRLQK", DateTime.Now.Date.AddHours(8) },
        };

        [Theory]
        [MemberData(nameof(ValidTokenData))]
        public void GetAccessToken_ValidInputs_ReturnsValidToken(Guid userID, string userName,
            IEnumerable<string> roles, string issuer, string audience, string secretKey, DateTime expirationDate)
        {
            // Arrange

            // Act
            var jwt = _jwtTokenServiceSUT.GetAccessToken(userID, userName, roles, issuer, audience, secretKey, expirationDate);

            // Assert
            Assert.True(!string.IsNullOrEmpty(jwt));

            var jwtSecurityToken = new JwtSecurityTokenHandler().ReadToken(jwt) as JwtSecurityToken;

            Assert.NotNull(jwtSecurityToken.Claims
                .SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier && c.Value == userID.ToString()));
            Assert.NotNull(jwtSecurityToken.Claims
                .SingleOrDefault(c => c.Type == ClaimTypes.Name && c.Value == userName));
            Assert.True(roles.All(r => jwtSecurityToken.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .Contains(r)));

            Assert.Equal(SecurityAlgorithms.HmacSha256, jwtSecurityToken.SignatureAlgorithm);

            var expiryClaim = jwtSecurityToken.Claims.SingleOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp);
            Assert.NotNull(expiryClaim);

            var tokenExpirationDate = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(expiryClaim.Value));
            Assert.Equal(expirationDate, tokenExpirationDate.LocalDateTime);

            var issuerClaim = jwtSecurityToken.Claims.SingleOrDefault(c => c.Type == JwtRegisteredClaimNames.Iss);
            if (string.IsNullOrEmpty(issuer))
            {
                Assert.Null(issuerClaim);
            }
            else
            {
                Assert.NotNull(issuerClaim);
                Assert.Equal(issuer, issuerClaim.Value);
            }

            var audienceClaim = jwtSecurityToken.Claims.SingleOrDefault(c => c.Type == JwtRegisteredClaimNames.Aud);
            if (string.IsNullOrEmpty(audience))
            {
                Assert.Null(audienceClaim);
            }
            else
            {
                Assert.NotNull(audienceClaim);
                Assert.Equal(audience, audienceClaim.Value);
            }
        }
    }
}
