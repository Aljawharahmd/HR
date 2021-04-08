using HR.Application.Services.Abstraction;
using System.Security.Cryptography;
using System.Text;

namespace HR.Application.Services
{
    public class ProtectionService : IProtectionService
    {
        public bool VerifyHashedPassword(string plaintText, string hash)
        {
            return hash.Equals(ComputeHash(plaintText));
        }

        public string ComputeHash(string plaintText)
        {
            var stringBuilder = new StringBuilder();

            using var algorithm = SHA256.Create();
            var hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(plaintText));

            foreach (var b in hash)
                stringBuilder.Append(b.ToString("X2"));

            return stringBuilder.ToString();
        }
    }
}
