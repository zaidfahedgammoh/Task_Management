using Task_Management.Application.Interfaces;
using Task_Management.Domain;

namespace Task_Management_Infrastructure.Services;

public class PasswordHasher : IPasswordHasher
{
    private readonly Microsoft.AspNetCore.Identity.IPasswordHasher<User> _hasher;

    public PasswordHasher()
    {
        _hasher = new Microsoft.AspNetCore.Identity.PasswordHasher<User>();
    }

    public string Hash(string password)
    {
        return _hasher.HashPassword(null!, password);
    }

    public bool Verify(string password, string passwordHash)
    {
        var result = _hasher.VerifyHashedPassword(
            null!,
            passwordHash,
            password);

        return result != Microsoft.AspNetCore.Identity.PasswordVerificationResult.Failed;
    }
}