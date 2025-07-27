using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Infrastructure.HashPassword
{
    public class PasswordHasher : IPasswordHasher
    {
        /// <summary>
        /// Hashes a password using PBKDF2 with SHA-256.
        /// </summary>
        /// <param name="password"> The password to hash.</param>
        /// <returns> A Base64-encoded string containing the salt and hash.</returns>
        public string HashPassword(string password)
        {
            //Generate a salt
            byte[] salt = RandomNumberGenerator.GetBytes(16);

            //Derive a key from the password using PBKDF2
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32);

            //Combine the salt and hash into a single byte array
            byte[] hashBytes = new byte[48];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 32);

            //Convert to Base64 string
            return Convert.ToBase64String(hashBytes);
        }
        /// <summary>
        /// Verifies a password against a hashed password.
        /// </summary>
        /// <param name="hashedPassword"> The hashed password to verify against.</param>
        /// <param name="password"> The password to verify.</param>
        /// <returns> True if the password matches the hash; otherwise, false.</returns>
        public bool VerifyPassword(string hashedPassword, string password)
        {
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);

            //Extract the salt from the hash
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            //Derive the hash from the provided password using the same salt
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32);

            //Compare the derived hash with the original hash
            for (int i = 0; i < 32; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
