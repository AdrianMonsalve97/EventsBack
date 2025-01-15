using System.Text.RegularExpressions;

namespace EventsApi.Helpers
{
    public static class PasswordValidator
    {
        public static bool IsValid(string password, out string errorMessage)
        {
            errorMessage = string.Empty;
            // Longitud mínima
            if (password.Length < 8)
            {
                errorMessage = "La contraseña debe tener al menos 8 caracteres.";
                return false;
            }
            // Al menos una letra mayúscula
            if (!Regex.IsMatch(password, "[A-Z]"))
            {
                errorMessage = "La contraseña debe contener al menos una letra mayúscula.";
                return false;
            }
            // Al menos una letra minúscula
            if (!Regex.IsMatch(password, "[a-z]"))
            {
                errorMessage = "La contraseña debe contener al menos una letra minúscula.";
                return false;
            }
            // Al menos un número
            if (!Regex.IsMatch(password, "[0-9]"))
            {
                errorMessage = "La contraseña debe contener al menos un número.";
                return false;
            }
            // Al menos un carácter especial
            if (!Regex.IsMatch(password, @"[\@\#\!\$\%\^\&\*\(\)_\+\-\=\{\}\[\]\:\;\""\<\>\,\.\?\|\/\\]"))
            {
                errorMessage = "La contraseña debe contener al menos un carácter especial.";
                return false;
            }
            // Sin espacios
            if (password.Contains(" "))
            {
                errorMessage = "La contraseña no debe contener espacios.";
                return false;
            }
            return true;
        }
    }
}
