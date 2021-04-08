namespace HR.Application.Services.Abstraction
{
    public interface IProtectionService
    {
        bool VerifyHashedPassword(string plainText, string hash);
        string ComputeHash(string plainText);
    }
}
