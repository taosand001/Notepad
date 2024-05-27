using Notepad.Business.Interfaces;
using System.Security.Cryptography;

namespace Notepad.Business.Service
{
    public class PasswordHashingService : IPasswordHashingService
    {
        public bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using var hmac = new HMACSHA512(storedSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(storedHash);
        }
    }
}
