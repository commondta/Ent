using System;
using System.Security.Cryptography;

namespace BusinessLayer
{
    /// <summary>
    /// PBKDF2 password hashing (Rfc2898DeriveBytes).
    /// Stored format: PBKDF2$&lt;iterations&gt;$&lt;saltBase64&gt;$&lt;hashBase64&gt;
    /// </summary>
    public static class PasswordHasher
    {
        const int SaltSize = 16;      // bytes
        const int HashSize = 32;      // bytes
        const int Iterations = 100000;
        const string Prefix = "PBKDF2";

        public static string Hash(string password)
        {
            if (password == null) throw new ArgumentNullException("password");

            byte[] salt = new byte[SaltSize];
            using (var rng = new RNGCryptoServiceProvider())
                rng.GetBytes(salt);

            byte[] hash = Derive(password, salt, Iterations);
            return string.Format("{0}${1}${2}${3}", Prefix, Iterations,
                Convert.ToBase64String(salt), Convert.ToBase64String(hash));
        }

        public static bool Verify(string password, string stored)
        {
            if (password == null || string.IsNullOrEmpty(stored)) return false;

            string[] parts = stored.Split('$');
            if (parts.Length != 4 || parts[0] != Prefix) return false;

            int iterations;
            if (!int.TryParse(parts[1], out iterations) || iterations < 1) return false;

            byte[] salt, expected;
            try
            {
                salt = Convert.FromBase64String(parts[2]);
                expected = Convert.FromBase64String(parts[3]);
            }
            catch (FormatException) { return false; }

            byte[] actual = Derive(password, salt, iterations);
            return FixedTimeEquals(actual, expected);
        }

        static byte[] Derive(string password, byte[] salt, int iterations)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations))
                return pbkdf2.GetBytes(HashSize);
        }

        static bool FixedTimeEquals(byte[] a, byte[] b)
        {
            if (a == null || b == null || a.Length != b.Length) return false;
            int diff = 0;
            for (int i = 0; i < a.Length; i++)
                diff |= a[i] ^ b[i];
            return diff == 0;
        }
    }
}
