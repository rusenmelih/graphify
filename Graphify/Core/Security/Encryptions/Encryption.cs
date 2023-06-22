using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Core.Security.Encryptions
{
    public static class Encryption
    {
        public static string SHA512InBase64(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            using (var hash = System.Security.Cryptography.SHA512.Create())
            {
                byte[] hashedInputBytes = hash.ComputeHash(bytes);
                return Convert.ToBase64String(hashedInputBytes);
            }
        }
        public static string CreateSalt(int size)
        {
            byte[] buff = new byte[size];
            RandomNumberGenerator.Fill(buff);
            return Convert.ToBase64String(buff);
        }
        public static string SHA512Hashing(string value, string salt)
        {
            string hash = Encryption.SHA512InBase64(value);
            hash = String.Concat(hash, salt);
            return Encryption.SHA512InBase64(hash);
        }
    }
}
