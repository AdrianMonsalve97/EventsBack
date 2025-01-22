using EventsApi.Application.DTO;
using EventsApi.Domain.Entities;
using EventsApi.Domain.Enums;
using EventsApi.Models;
using EventsApi.Models.DTO;
using EventsApi.Models.Enums;
using EventsApi.Repositorio;
using Models.resgeneral;

public class UsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    // Obtener todos los usuarios
    public async Task<RespuestaGeneral<List<UsuarioDto>>> ObtenerTodosLosUsuariosAsync()
    {
        try
        {
            IEnumerable<Usuario> usuarios = await _usuarioRepository.GetAllAsync();
            List<UsuarioDto> usuariosDto = usuarios.Select(u =>
            {
                return new UsuarioDto
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    CorreoCorporativo = u.CorreoCorporativo,
                    CorreoPersonal = u.CorreoPersonal,
                    Rol = u.Rol.ToString(),
                    TipoDocumento = u.TipoDocumento.ToString(),
                    DocumentoIdentidad = u.DocumentoIdentidad,
                    CelularPersonal = u.CelularPersonal,
                    CelularCorporativo = u.CelularCorporativo,
                    FechaContratoInicio = u.FechaContratoInicio.HasValue ? u.FechaContratoInicio.Value : default(DateTime),
                    FechaContratoFin = u.FechaContratoFin.HasValue ? u.FechaContratoFin.Value : default(DateTime),
                    NombreEmpresa = u.Empresa?.NombreEmpresa
                };
            }).ToList();

            return new RespuestaGeneral<List<UsuarioDto>>
            {
                Error = false,
                Mensaje = "Usuarios obtenidos con éxito.",
                Resultado = usuariosDto
            };
        }
        catch (Exception ex)
        {
            // Registrar el error (puedes usar ILogger si está configurado)
            Console.WriteLine($"Error: {ex.Message}");
            return new RespuestaGeneral<List<UsuarioDto>>
            {
                Error = true,
                Mensaje = "Ocurrió un error al obtener los usuarios.",
                Resultado = null
            };
        }
    }

    // Actualizar un usuario
    public async Task<RespuestaGeneral<object>> ActualizarUsuarioAsync(int id, Usuario usuarioActualizado)
    {
        Usuario? usuarioExistente = await _usuarioRepository.GetByIdAsync(id);

        if (usuarioExistente == null)
        {
            return new RespuestaGeneral<object>
            {
                Error = true,
                Mensaje = "Usuario no encontrado.",
                Resultado = null
            };
        }

        // Validar el rol antes de realizar cambios
        if (usuarioActualizado.Empresa != null &&
            (usuarioActualizado.Rol != Rol.Logistico &&
             usuarioActualizado.Rol != Rol.Ejecutivo &&
             usuarioActualizado.Rol != Rol.Cliente &&
             usuarioActualizado.Rol != Rol.Aprobador))
        {
            return new RespuestaGeneral<object>
            {
                Error = true,
                Mensaje = "El rol del usuario no permite asociarlo a una empresa.",
                Resultado = null
            };
        }

        if (usuarioActualizado.Rol == Rol.Administrador && usuarioActualizado.Empresa.NombreEmpresa != null)
        {
            return new RespuestaGeneral<object>
            {
                Error = true,
                Mensaje = "Un administrador no puede estar asociado a una empresa.",
                Resultado = null
            };
        }

        // Actualizar los campos permitidos
        usuarioExistente.Nombre = usuarioActualizado.Nombre;
        usuarioExistente.CorreoCorporativo = usuarioActualizado.CorreoCorporativo;
        usuarioExistente.Rol = usuarioActualizado.Rol;
        usuarioExistente.PasswordHash = usuarioActualizado.PasswordHash;
        usuarioExistente.Empresa.NombreEmpresa = usuarioActualizado.Empresa.NombreEmpresa;

        try
        {
            await _usuarioRepository.UpdateAsync(usuarioExistente);
            return new RespuestaGeneral<object>
            {
                Error = false,
                Mensaje = "Usuario actualizado con éxito.",
                Resultado = null
            };
        }
        catch (Exception ex)
        {
            // Registrar el error
            Console.WriteLine($"Error al actualizar usuario: {ex.Message}");
            return new RespuestaGeneral<object>
            {
                Error = true,
                Mensaje = "Ocurrió un error al actualizar el usuario.",
                Resultado = null
            };
        }
    }

    // Método privado para convertir un Usuario a UsuarioDto
    private UsuarioInscritoDto ConvertirUsuarioADto(Usuario usuario)
    {
        return new UsuarioInscritoDto
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            CorreoCorporativo = usuario.CorreoCorporativo,
            Rol = usuario.Rol.ToString(),
            EventosCreados = usuario.EventosCreados.Select(e => new EventoDto
            {
                Id = e.Id,
                Nombre = e.Nombre,
                FechaHora = e.FechaHora,
                Ubicacion = e.Ubicacion,
                CapacidadMaxima = e.CapacidadMaxima,
                AsistentesRegistrados = e.AsistentesRegistrados
            }).ToList(),
            Inscripciones = usuario.Inscripciones.Select(i => new InscripcionDto
            {
                EventoId = i.EventoId,
                EventoNombre = i.Evento.Nombre,
                FechaInscripcion = i.FechaInscripcion
            }).ToList()
        };
    }
}
