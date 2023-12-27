using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TeamViewerLogReader.Service
{
    public static class PasswordHasher
    {
        private const int Pbkdf2Iterations = 10000;
        private const int SaltSize = 16; 
        private const int HashSize = 20; 

        public static string HashPassword(string password)
        {
            //Salt creation
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

            // hash creation
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Pbkdf2Iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            // salt and hash combination
            byte[] hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            // Converting to Base64
            return Convert.ToBase64String(hashBytes);
        }

        public static bool VerifyPassword(string storedHash, string password)
        {
            // Extract the byte array from the stored hash
            byte[] hashBytes = Convert.FromBase64String(storedHash);

            // Extracts the salt from the hash
            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            // Calculates password hash
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Pbkdf2Iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            // Compare the calculated hash with the stored hash
            for (int i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                {
                    return false;
                }
            }

            return true;
        }
    }

}
