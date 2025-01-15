using EventsApi.Data;
using EventsApi.Models;
using EventsApi.Helpers;
using Microsoft.EntityFrameworkCore;
using Models.resgeneral;
using System.Security.Cryptography;
using System.Text;
using EventsApi.Domain.Entities;
using EventsApi.Models.Enums;

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
            var passwordValidation = ValidatePassword(usuario.PasswordHash);
            if (!passwordValidation.IsValid)
            {
                return CreateErrorResponse<object>(passwordValidation.ErrorMessage);
            }

            // Verificar si el correo ya está registrado
            if (await IsEmailRegistered(usuario.Correo))
            {
                return CreateErrorResponse<object>("El correo ya está registrado.");
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
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == loginDto.Correo);

            if (user == null)
            {
                return CreateErrorResponse<string>("Correo no registrado.");
            }

            if (user.PasswordHash != HashPassword(loginDto.Password))
            {
                return CreateErrorResponse<string>("Contraseña incorrecta.");
            }

            // Generar el token JWT
            string token = _jwtHelper.GenerateToken(user.Id, ((Rol)user.Rol).GetEnumMemberValue());


            return new RespuestaGeneral<string>
            {
                Error = false,
                Mensaje = "Inicio de sesión exitoso.",
                Resultado = token
            };
        }

        private (bool IsValid, string ErrorMessage) ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return (false, "La contraseña no puede ser nula o vacía.");
            }

            return PasswordValidator.IsValid(password, out string errorMessage) ? (true, string.Empty) : (false, errorMessage);
        }

        private async Task<bool> IsEmailRegistered(string email)
        {
            return await _context.Usuarios.AnyAsync(u => u.Correo == email);
        }

        private string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("La contraseña no puede ser nula o vacía.", nameof(password));
            }

            using var sha256 = SHA256.Create();
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private RespuestaGeneral<T> CreateErrorResponse<T>(string errorMessage)
        {
            return new RespuestaGeneral<T>
            {
                Error = true,
                Mensaje = errorMessage,
                Resultado = default
            };
        }
    }
}
