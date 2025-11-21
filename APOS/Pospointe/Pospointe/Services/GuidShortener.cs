using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pospointe.Services
{
    public class GuidShortener
    {
        public static string ShortenGuid(Guid guid)
        {
            // Convert the GUID to a byte array
            byte[] bytes = guid.ToByteArray();

            // Use Base32 encoding (custom encoding with only uppercase letters and numbers)
            const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder result = new StringBuilder();

            // Iterate over the byte array and map to Base32 characters
            foreach (var b in bytes)
            {
                result.Append(characters[b % characters.Length]);
            }

            // Return the first 6 characters
            return result.ToString().Substring(0, 6);
        }
    }
}
