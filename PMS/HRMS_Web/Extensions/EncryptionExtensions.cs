using System.Security.Cryptography;
using System.Text;

namespace HRMS_Web.Extensions
{
    public static class EncryptionExtensions
    {
        // Must be 32 bytes (256 bits)
        private static readonly string Key = "12345678901234567890123456789012"; // ✅ Exactly 32 characters
                                                                                 // Must be 16 bytes (128 bits)
        private static readonly string IV = "1234567890123456"; // ✅ Exactly 16 characters

        public static string Encrypt(this string plainText)
        {
            if (string.IsNullOrWhiteSpace(plainText))
                return plainText;

            using Aes aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(Key);
            aes.IV = Encoding.UTF8.GetBytes(IV);

            using MemoryStream ms = new();
            using CryptoStream cs = new(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
            using (StreamWriter sw = new(cs)) // fixed here
            {
                sw.Write(plainText);
            }

            return Convert.ToBase64String(ms.ToArray());
        }


        public static string Decrypt(this string encryptedText)
        {
            if (string.IsNullOrWhiteSpace(encryptedText))
                return encryptedText;

            using Aes aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(Key);
            aes.IV = Encoding.UTF8.GetBytes(IV);

            byte[] buffer = Convert.FromBase64String(encryptedText);

            using MemoryStream ms = new(buffer);
            using CryptoStream cs = new(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using (StreamReader sr = new(cs)) // use braces here as well
            {
                return sr.ReadToEnd();
            }
        }
    }

}
