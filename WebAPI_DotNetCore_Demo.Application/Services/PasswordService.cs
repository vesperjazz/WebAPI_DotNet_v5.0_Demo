using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using WebAPI_DotNetCore_Demo.Application.Services.Interfaces;

namespace WebAPI_DotNetCore_Demo.Application.Services
{
    public class PasswordService : IPasswordService
    {
        // https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.hmac?view=netframework-4.7.2
        private readonly HMAC _hmac;

        public PasswordService(HMAC hmac)
        {
            _hmac = hmac ?? throw new ArgumentNullException(nameof(hmac));
        }

        public (byte[] Salt, byte[] Hash) CreatePasswordHash(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password cannot be null, empty or white space.");
            }

            return (_hmac.Key, _hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }

        public bool VerifyPassword(string password, byte[] storedPasswordSalt, byte[] storedPasswordHash)
        {
            if (storedPasswordHash.Length != 64)
            {
                throw new ArgumentException("Invalid length of password hash (64 bytes expected).");
            }

            if (storedPasswordSalt.Length != 128)
            {
                throw new ArgumentException("Invalid length of password salt (128 bytes expected).");
            }

            // The stored salt has to be assigned back to the HMAC to re-compute the correct password hash.
            _hmac.Key = storedPasswordSalt;

            return _hmac.ComputeHash(Encoding.UTF8.GetBytes(password)).SequenceEqual(storedPasswordHash);
        }
    }
}
