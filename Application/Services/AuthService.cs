using System.Runtime.InteropServices.JavaScript;
using EventsApi.Data;
using EventsApi.Models;
using EventsApi.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using EventsApi.Application.DTO;
using EventsApi.Domain.Entities;
using EventsApi.Domain.Enums;
using EventsApi.Models.Enums;
using Models.resgeneral;
using BCrypt.Net;


namespace EventsApi.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly JwtTokenHelper _jwtHelper;
        private readonly PasswordService _passwordService;

        public AuthService(AppDbContext context, JwtTokenHelper jwtHelper, PasswordService passwordService)
        {
            _context = context;
            _jwtHelper = jwtHelper;
            _passwordService = passwordService;
        }

public async Task<RespuestaGeneral<object>> Register(UsuarioDto usuarioDto)
{
    try
    {
        // Validación de enums dentro del servicio
        if (!Enum.TryParse(usuarioDto.TipoDocumento.ToString(), out TipoDocumento tipoDocumento))
        {
            return new RespuestaGeneral<object>
            {
                Error = true,
                Mensaje = $"Tipo de documento inválido: {usuarioDto.TipoDocumento}",
                Resultado = null
            };
        }

        if (!Enum.TryParse(usuarioDto.Rol.ToString(), out Rol rol))
        {
            return new RespuestaGeneral<object>
            {
                Error = true,
                Mensaje = $"Rol inválido: {usuarioDto.Rol}",
                Resultado = null
            };
        }

        // Verificar si el usuario existe
        Usuario usuarioExistente = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.DocumentoIdentidad == usuarioDto.DocumentoIdentidad);

        if (usuarioExistente != null)
        {
            return new RespuestaGeneral<object>
            {
                Error = true,
                Mensaje = "El usuario ya existe con ese documento de identidad.",
                Resultado = null
            };
        }

        // Crear una instancia del servicio de contraseña
        PasswordService passwordService = new PasswordService();

        // Generar el salt
        string salt = passwordService.GenerateSalt();

        // Generar el hash para la contraseña
        string passwordHash = passwordService.HashPassword(usuarioDto.PasswordHash, salt);

        // Crear el usuario
        Usuario usuario = new Usuario
        {
            Nombre = usuarioDto.Nombre,
            CelularPersonal = usuarioDto.CelularPersonal,
            CelularCorporativo = usuarioDto.CelularCorporativo,
            TipoDocumento = tipoDocumento,
            DocumentoIdentidad = usuarioDto.DocumentoIdentidad,
            CorreoCorporativo = usuarioDto.CorreoCorporativo,
            CorreoPersonal = usuarioDto.CorreoPersonal,
            Rol = rol,
            FechaContratoInicio = usuarioDto.FechaContratoInicio,
            FechaContratoFin = usuarioDto.FechaContratoFin,
            PasswordHash = passwordHash, // Asignar el hash de la contraseña
            PasswordSalt = salt // Almacenar el salt en la base de datos
        };

        // Guardar el usuario en la base de datos
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return new RespuestaGeneral<object>
        {
            Error = false,
            Mensaje = "Usuario creado con éxito.",
            Resultado = usuario
        };
    }
    catch (Exception ex)
    {
        return new RespuestaGeneral<object>
        {
            Error = true,
            Mensaje = "Error al registrar el usuario.",
            Resultado = ex.Message
        };
    }
}
public async Task<RespuestaGeneral<LoginResponseDto>> LoginAsync(LoginDto loginDto)
{
    // Buscar al usuario por correo
    Usuario user = await _context.Usuarios.FirstOrDefaultAsync(u => u.CorreoCorporativo == loginDto.CorreoCorporativo);

    if (user == null)
    {
        return CreateErrorResponse<LoginResponseDto>("Correo no registrado.");
    }

    // Verificar si la contraseña es correcta
    bool passwordValid = _passwordService.VerifyPassword(loginDto.Password, user.PasswordHash, user.PasswordSalt);
    if (!passwordValid)
    {
        return CreateErrorResponse<LoginResponseDto>("Contraseña incorrecta.");
    }

    // Validar y convertir el rol
    if (!Enum.IsDefined(typeof(Rol), user.Rol))
    {
        return CreateErrorResponse<LoginResponseDto>("Rol del usuario no es válido.");
    }

    Rol userRole = (Rol)user.Rol;
    string roleValue = EnumExtensions.GetEnumMemberValue(userRole);

    // Generar el token JWT
    string token = _jwtHelper.GenerateToken(user.Id, userRole);

    // Crear la respuesta que incluye el token y los datos del usuario
    var response = new LoginResponseDto
    {
        Token = token,
        UsuarioId = user.Id,
        UsuarioNombre = user.Nombre
    };

    // Retornar la respuesta con la información adicional
    return new RespuestaGeneral<LoginResponseDto>
    {
        Error = false,
        Mensaje = "Inicio de sesión exitoso.",
        Resultado = response // Retornar un objeto con el token y los datos del usuario
    };
}

        private (bool IsValid, string ErrorMessage) ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return (false, "La contraseña no puede ser nula o vacía.");
            }

            string errorMessage;
            bool isValid = PasswordValidator.IsValid(password, out errorMessage);
            return isValid ? (true, string.Empty) : (false, errorMessage);
        }

        private async Task<bool> IsEmailRegistered(string email)
        {
            return await _context.Usuarios.AnyAsync(u => u.CorreoCorporativo == email);
        }

        private RespuestaGeneral<T> CreateErrorResponse<T>(string errorMessage)
        {
            return new RespuestaGeneral<T>
            {
                Error = true,
                Mensaje = errorMessage,
                Resultado = default(T)
            };
        }
    }
}
