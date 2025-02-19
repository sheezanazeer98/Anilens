using AniLens.Core.Interfaces;
using AniLens.Shared;
using DevOne.Security.Cryptography.BCrypt;

namespace AniLens.Core.Services;

public class HashService : IHashService
{
    public Result<string> HashPassword(string password)
    {
        if (string.IsNullOrEmpty(password) ||
            string.IsNullOrWhiteSpace(password))
            return Result<string>.Failure(Error.Parameter
                .ToDescriptionString());
        
        var salt = BCryptHelper.GenerateSalt();
        var hashedPassword = BCryptHelper.HashPassword(password, salt);

        return hashedPassword != null
            ? Result<string>.Success(hashedPassword)
            : Result<string>.Failure(Error.Internal.ToDescriptionString());
    }

    public bool CheckPassword(string password, string hashedPassword)
    {
        if (string.IsNullOrEmpty(password) ||
            string.IsNullOrWhiteSpace(password))
            return false;
        if (string.IsNullOrEmpty(hashedPassword) ||
            string.IsNullOrWhiteSpace(hashedPassword) || 
            hashedPassword.Length != 60)
            return false;
        
        return !BCryptHelper.CheckPassword(password, hashedPassword);
    }
}