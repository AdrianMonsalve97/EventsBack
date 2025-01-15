using EventsApi.Domain.Entities;
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
    public async Task<RespuestaGeneral<IEnumerable<Usuario>>> ObtenerTodosLosUsuariosAsync()
    {
        try
        {
            IEnumerable<Usuario> usuarios = await _usuarioRepository.GetAllAsync();
            return new RespuestaGeneral<IEnumerable<Usuario>>
            {
                Error = false,
                Mensaje = "Usuarios obtenidos con éxito.",
                Resultado = usuarios
            };
        }
        catch (Exception ex)
        {
            return new RespuestaGeneral<IEnumerable<Usuario>>
            {
                Error = true,
                Mensaje = "Ocurrió un error al obtener los usuarios.",
                Resultado = null
            };
        }
    }

    public async Task<RespuestaGeneral<IEnumerable<UsuarioDto>>> ObtenerListaUsuariosAsync()
    {
        // Obtención de todos los usuarios
        IEnumerable<Usuario> usuarios = await _usuarioRepository.ObtenerTodosLosUsuariosAsync();

        // Conversión de usuarios a DTOs
        List<UsuarioDto> usuariosDto = new List<UsuarioDto>();
        foreach (Usuario u in usuarios)
        {
            UsuarioDto usuarioDto = new UsuarioDto
            {
                Id = u.Id,
                Nombre = u.Nombre,
                Correo = u.CorreoCorporativo,
                Rol = "Administrador",
                EventosCreados = new List<EventoDto>(),
                Inscripciones = new List<InscripcionDto>()
            };

            // Convertir eventos creados a DTOs
            foreach (Evento e in u.EventosCreados)
            {
                EventoDto eventoDto = new EventoDto
                {
                    Id = e.Id,
                    Nombre = e.Nombre,
                    FechaHora = e.FechaHora,
                    Ubicacion = e.Ubicacion,
                    CapacidadMaxima = e.CapacidadMaxima,
                    AsistentesRegistrados = e.AsistentesRegistrados
                };
                usuarioDto.EventosCreados.Add(eventoDto);
            }

            // Convertir inscripciones a DTOs
            foreach (Inscripcion i in u.Inscripciones)
            {
                InscripcionDto inscripcionDto = new InscripcionDto
                {
                    EventoId = i.EventoId,
                    EventoNombre = i.Evento.Nombre,
                    FechaInscripcion = i.FechaInscripcion
                };
                usuarioDto.Inscripciones.Add(inscripcionDto);
            }

            usuariosDto.Add(usuarioDto);
        }

        return new RespuestaGeneral<IEnumerable<UsuarioDto>>
        {
            Error = false,
            Mensaje = "Usuarios obtenidos con éxito.",
            Resultado = usuariosDto
        };
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

        // Actualizar los campos permitidos
        usuarioExistente.Nombre = usuarioActualizado.Nombre;
        usuarioExistente.CorreoCorporativo = usuarioActualizado.CorreoCorporativo;
        usuarioExistente.Rol = usuarioActualizado.Rol;
        usuarioExistente.PasswordHash = usuarioActualizado.PasswordHash;

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
        catch (Exception)
        {
            return new RespuestaGeneral<object>
            {
                Error = true,
                Mensaje = "Ocurrió un error al actualizar el usuario.",
                Resultado = null
            };
        }
    }
}
