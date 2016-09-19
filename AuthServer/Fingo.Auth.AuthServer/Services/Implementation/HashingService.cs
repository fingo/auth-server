using System;
using Fingo.Auth.AuthServer.Services.Interfaces;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Fingo.Auth.DbAccess.Repository.Interfaces;

namespace Fingo.Auth.AuthServer.Services.Implementation
{
    public class HashingService : IHashingService
    {
        private readonly IUserRepository _userRepository;
        private readonly int _iterations;

        public HashingService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _iterations = 131072;
        }

        public string HashWithGivenSalt(string password, string hexSalt)
        {
            return Hash(password, hexSalt);
        }

        public string HashWithDatabaseSalt(string password, string userLogin)
        {
            var hexSalt = _userRepository.GetAll().FirstOrDefault(u => u.Login == userLogin).PasswordSalt;
            
            if (hexSalt == null)
            {
                throw new Exception($"Could not find password salt of user (login: {userLogin}).");
            }

            return Hash(password, hexSalt);
        }

        public string GenerateNewSalt()
        {
            var saltBytes = Guid.NewGuid().ToByteArray();  
            return BytesToHexString(saltBytes);
        }

        private string Hash(string password, string hexSalt)
        {
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(password), HexStringToBytes(hexSalt), _iterations);
            return BytesToHexString(rfc2898DeriveBytes.GetBytes(20));
        }

        private byte[] HexStringToBytes(string hexString)
        {
            return Enumerable
                    .Range(0, hexString.Length)
                    .Where(x => x % 2 == 0)
                    .Select(x => Convert.ToByte(hexString.Substring(x, 2), 16))
                    .ToArray();
        }

        private string BytesToHexString(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", string.Empty).ToLowerInvariant();
        }
    }
}