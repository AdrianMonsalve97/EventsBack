using System;
using System.Security.Cryptography;
using System.Text;

namespace EventsApi.Services
{
    public class PasswordService
    {
        private const int SaltSize = 16; // Tamaño en bytes
        private const int HashSize = 32; // Tamaño del hash en bytes
        private const int Iterations = 10000; // Iteraciones de PBKDF2

        /// <summary>
        /// Genera un salt aleatorio para la contraseña.
        /// </summary>
        /// <returns>Salt generado como una cadena Base64.</returns>
        public string GenerateSalt()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] salt = new byte[SaltSize];
                rng.GetBytes(salt);
                return Convert.ToBase64String(salt);
            }
        }

        /// <summary>
        /// Genera un hash seguro para la contraseña utilizando PBKDF2.
        /// </summary>
        /// <param name="password">Contraseña en texto plano.</param>
        /// <param name="salt">Salt para el hash.</param>
        /// <returns>Hash generado como una cadena Base64.</returns>
        public string HashPassword(string password, string salt)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("La contraseña no puede ser nula o vacía.", nameof(password));

            if (string.IsNullOrEmpty(salt))
                throw new ArgumentException("El salt no puede ser nulo o vacío.", nameof(salt));

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), Iterations, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(HashSize);
                return Convert.ToBase64String(hash);
            }
        }

        /// <summary>
        /// Verifica si una contraseña coincide con el hash almacenado.
        /// </summary>
        /// <param name="password">Contraseña en texto plano.</param>
        /// <param name="storedHash">Hash almacenado.</param>
        /// <param name="salt">Salt utilizado para generar el hash.</param>
        /// <returns>True si la contraseña coincide; de lo contrario, false.</returns>
        public bool VerifyPassword(string password, string storedHash, string salt)
        {
            if (string.IsNullOrEmpty(storedHash))
                throw new ArgumentException("El hash almacenado no puede ser nulo o vacío.", nameof(storedHash));

            string computedHash = HashPassword(password, salt);
            return storedHash == computedHash;
        }
    }
}
