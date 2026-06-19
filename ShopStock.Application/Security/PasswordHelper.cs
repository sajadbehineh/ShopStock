using System.Security.Cryptography;

namespace ShopStock.Application.Security
{
    public static class PasswordHelper
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 100_000;

        private const char Separator = '.';

        public static string HashPassword(this string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return string.Empty;

            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password: password,
                salt: salt,
                iterations: Iterations,
                hashAlgorithm: HashAlgorithmName.SHA256,
                outputLength: KeySize);

            return string.Join(
                Separator,
                "PBKDF2",
                Iterations,
                Convert.ToBase64String(salt),
                Convert.ToBase64String(hash));
        }

        // Verify if a plain password matches the hashed password
        public static bool VerifyPassword(string plainPassword, string hashedPassword)
        {
            if (string.IsNullOrWhiteSpace(plainPassword) || string.IsNullOrWhiteSpace(hashedPassword))
                return false;

            string[] parts = hashedPassword.Split(Separator);

            if (parts.Length != 4)
                return false;

            if (parts[0] != "PBKDF2")
                return false;

            if (!int.TryParse(parts[1], out int iterations))
                return false;

            byte[] salt;
            byte[] storedHash;

            try
            {
                salt = Convert.FromBase64String(parts[2]);
                storedHash = Convert.FromBase64String(parts[3]);
            }
            catch
            {
                return false;
            }

            byte[] computedHash = Rfc2898DeriveBytes.Pbkdf2(
                password: plainPassword,
                salt: salt,
                iterations: iterations,
                hashAlgorithm: HashAlgorithmName.SHA256,
                outputLength: storedHash.Length);

            return CryptographicOperations.FixedTimeEquals(computedHash, storedHash);
        }
    }
}
