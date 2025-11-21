using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Pospointe
{
    class EncryptionHelper
    {
        private static readonly string EncryptionKey = "R9a3VjNu5J8Q0oNj3V1XZzO1L9pQiB4M5z5Ht5GJNRg="; // Use a secure key, 32 chars for AES-256

        public static string EncryptString(string plainText)
        {
            // Convert the Base64 encoded key to a byte array
            byte[] key = Convert.FromBase64String(EncryptionKey);

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.GenerateIV(); // Generates a random Initialization Vector (IV)
                byte[] iv = aes.IV;

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    memoryStream.Write(iv, 0, iv.Length); // Prepend IV to the encrypted data

                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }
                    }

                    return Convert.ToBase64String(memoryStream.ToArray());
                }
            }

        }
    }
}
