using System;
using System.Security.Cryptography;
using System.Text;
using WebAPI_DotNetCore_Demo.Application.Services;
using Xunit;

namespace WebAPI_DotNetCore_Demo.Application.Tests
{
    public class PasswordServiceTests : IDisposable
    {
        // https://stackoverflow.com/questions/1134671/how-can-i-safely-convert-a-byte-array-into-a-string-and-back
        private const string ComplexPassword123Salt = "Xg27Yrq+6B2MVLn3RzKjkgNPRWHvQFFDYzoJ/fDiihkmhr7u+bYNht4Z5I+/zI1/SvwPoMwlddXXKzZvskhU4daWHDdxEExBNuvhlQPmgHz3Cky115iHrtcyKYLFq4SeSAXc5qflG/p2JKbv5VSA+dD5RBC5Dr4wLObcXSYnFis=";
        private const string ComplexPassword123Hash = "K+2ZX2Ld7ytvHzTCEn4PDUXKqS7RsH1mwhBlpfyxdeD2KYmag8T96Us98ppzrwXwrJFhQ4VSZxxVnxpxujuukA==";
        private const string OneComplexPassword39Salt = "Z9UrrbyYVbhJsTZLPpwYOobsxJALLTcbUNMMHD7WnG2C+2/0UtqL/xvc0Fd/Vx5zvyyoE9ozWngywGs+x+sfSRl14tPjlytrPTYWLMCg3GnmckbmtYtiWgBSNwoi0VCvMr0bMdgHMqntOTQsy5kLGHxupVZH4iz9q97FTX/BRYM=";
        private const string OneComplexPassword39Hash = "hTeOIZqVapzZkvRDCI8Dwz7Xo3+ZBgfMZUL/GusASOxGBZDLM0+pCwjwbxdbr6W2b68reQG2xJ1lpG5vJLjwnA==";

        private readonly HMACSHA512 _hmac;

        public PasswordServiceTests()
        {
            _hmac = new HMACSHA512();
        }

        public void Dispose()
        {
            _hmac.Dispose();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void CreatePasswordHash_EmptyPassword_ThrowsArgumentException(string password)
        {
            var passwordService = new PasswordService(_hmac);

            Assert.Throws<ArgumentException>(() =>
            {
                passwordService.CreatePasswordHash(password);
            });
        }

        [Theory]
        [InlineData("ComplexPassword123")]
        [InlineData("OneComplexPassword39")]
        public void CreatePasswordHash_ProperPassword_ReturnsPasswordSaltAndHash(string password)
        {
            var expectedKey = _hmac.Key;
            var expectedHash = _hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            var passwordService = new PasswordService(_hmac);

            var (Salt, Hash) = passwordService.CreatePasswordHash(password);

            Assert.Equal(expectedKey, Salt);
            Assert.Equal(expectedHash, Hash);
        }

        [Theory]
        [InlineData("ComplexPassword123", 50, 25)]
        [InlineData("OneComplexPassword39", 22, 32)]
        [InlineData("VeryComplexPassword1", 64, 128)]
        public void VerifyPassword_IncorrectSaltHashLength_ThrowsArgumentException(string password, int saltLength, int hashLength)
        {
            var passwordService = new PasswordService(_hmac);

            Assert.Throws<ArgumentException>(() =>
            {
                var isPasswordCorrect = passwordService
                    .VerifyPassword(password, new byte[saltLength], new byte[hashLength]);
            });
        }

        [Theory]
        [InlineData("ComplexPassword123", ComplexPassword123Salt, ComplexPassword123Hash)]
        [InlineData("OneComplexPassword39", OneComplexPassword39Salt, OneComplexPassword39Hash)]
        public void VerifyPassword_CorrectPassword_ReturnsTrue(string password, string salt, string hash)
        {
            var passwordService = new PasswordService(_hmac);

            var isPasswordCorrect = passwordService.VerifyPassword(password,
                Convert.FromBase64String(salt),
                Convert.FromBase64String(hash));

            Assert.True(isPasswordCorrect);
        }

        [Theory]
        [InlineData("ComplexPassword456", ComplexPassword123Salt, ComplexPassword123Hash)]
        [InlineData("OneComplexPassword21", OneComplexPassword39Salt, OneComplexPassword39Hash)]
        public void VerifyPassword_WrongPassword_ReturnsFalse(string password, string salt, string hash)
        {
            var passwordService = new PasswordService(_hmac);

            var isPasswordCorrect = passwordService.VerifyPassword(password,
                Convert.FromBase64String(salt),
                Convert.FromBase64String(hash));

            Assert.False(isPasswordCorrect);
        }
    }
}
