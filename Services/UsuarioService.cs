using EventsApi.Models;
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
        IEnumerable<Usuario> usuarios = await _usuarioRepository.ObtenerTodosLosUsuariosAsync();

        var usuariosDto = usuarios.Select(u => new UsuarioDto
        {
            Id = u.Id,
            Nombre = u.Nombre,
            Correo = u.Correo,
            Rol = u.Rol,
            EventosCreados = u.EventosCreados.Select(e => new EventoDto
            {
                Id = e.Id,
                Nombre = e.Nombre,
                FechaHora = e.FechaHora,
                Ubicacion = e.Ubicacion,
                CapacidadMaxima = e.CapacidadMaxima,
                AsistentesRegistrados = e.AsistentesRegistrados
            }).ToList(),
            Inscripciones = u.Inscripciones.Select(i => new InscripcionDto
            {
                EventoId = i.EventoId,
                EventoNombre = i.Evento.Nombre,
                FechaInscripcion = i.FechaInscripcion
            }).ToList()
        });

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
        usuarioExistente.Correo = usuarioActualizado.Correo;
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
