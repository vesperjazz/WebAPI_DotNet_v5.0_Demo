namespace WebAPI_DotNetCore_Demo.Application.Services.Interfaces
{
    public interface IPasswordService
    {
        (byte[] Salt, byte[] Hash) CreatePasswordHash(string password);

        bool VerifyPassword(string password, byte[] storedPasswordSalt, byte[] storedPasswordHash);
    }
}
