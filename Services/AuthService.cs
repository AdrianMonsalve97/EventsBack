using EventsApi.Data;
using EventsApi.Models;
using EventsApi.Helpers;
using Microsoft.EntityFrameworkCore;
using Models.resgeneral;
using System.Security.Cryptography;
using System.Text;

namespace EventsApi.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly JwtTokenHelper _jwtHelper;

        public AuthService(AppDbContext context, JwtTokenHelper jwtHelper)
        {
            _context = context;
            _jwtHelper = jwtHelper;
        }

        public async Task<RespuestaGeneral<object>> RegisterAsync(Usuario usuario)
        {
            // Validar la contraseña
            if (!PasswordValidator.IsValid(usuario.PasswordHash, out string errorMessage))
            {
                return new RespuestaGeneral<object>
                {
                    Error = true,
                    Mensaje = errorMessage,
                    Resultado = null
                };
            }

            // Verificar si el correo ya está registrado
            bool correoExiste = await _context.Usuarios.AnyAsync(u => u.Correo == usuario.Correo);
            if (correoExiste)
            {
                return new RespuestaGeneral<object>
                {
                    Error = true,
                    Mensaje = "El correo ya está registrado.",
                    Resultado = null
                };
            }

            // Encriptar la contraseña
            usuario.PasswordHash = HashPassword(usuario.PasswordHash);

            // Registrar el usuario
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return new RespuestaGeneral<object>
            {
                Error = false,
                Mensaje = "Usuario registrado con éxito.",
                Resultado = null
            };
        }


        public async Task<RespuestaGeneral<string>> LoginAsync(LoginDto loginDto)
        {
            Usuario? user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == loginDto.Correo);

            if (user == null)
            {
                return new RespuestaGeneral<string>
                {
                    Error = true,
                    Mensaje = "Correo no registrado.",
                    Resultado = null
                };
            }

            if (user.PasswordHash != HashPassword(loginDto.Password))
            {
                return new RespuestaGeneral<string>
                {
                    Error = true,
                    Mensaje = "Contraseña incorrecta.",
                    Resultado = null
                };
            }

            string token = _jwtHelper.GenerateToken(user.Id, user.Rol);

            return new RespuestaGeneral<string>
            {
                Error = false,
                Mensaje = "Inicio de sesión exitoso.",
                Resultado = token
            };
        }

        private string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("La contraseña no puede ser nula o vacía.", nameof(password));
            }

            using System.Security.Cryptography.SHA256 sha256 = SHA256.Create();
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}
