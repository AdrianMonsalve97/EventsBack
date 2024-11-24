using EventsApi.Models;
using EventsApi.Repositorio;
using Models.resgeneral;

public class InscripcionService
{
    private readonly IInscriptionRepository _inscripcionRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IUsuarioRepository _usuarioRepository;

    public InscripcionService(IInscriptionRepository inscripcionRepository, IEventRepository eventRepository, IUsuarioRepository usuarioRepository)
    {
        _inscripcionRepository = inscripcionRepository;
        _eventRepository = eventRepository;
        _usuarioRepository = usuarioRepository;
    }
    public async Task<RespuestaGeneral<object>> RegistrarInscripcionAsync(int usuarioId, int eventoId)
    {
        // Obtener el evento
        Evento? evento = await _eventRepository.GetByIdAsync(eventoId);
        if (evento == null)
        {
            return new RespuestaGeneral<object>
            {
                Error = true,
                Mensaje = "El evento no existe.",
                Resultado = null
            };
        }
        if (evento.UsuarioCreadorId == usuarioId)
        {
            return new RespuestaGeneral<object>
            {
                Error = true,
                Mensaje = "No puedes inscribirte a un evento que tú mismo has creado.",
                Resultado = null
            };
        }
        int inscripcionesActuales = await _inscripcionRepository.CountUserInscripcionesAsync(usuarioId);
        if (inscripcionesActuales >= 3)
        {
            return new RespuestaGeneral<object>
            {
                Error = true,
                Mensaje = "No puedes inscribirte en más de 3 eventos.",
                Resultado = null
            };
        }

        // Verificar si el evento tiene capacidad disponible
        if (!evento.TieneCupo())
        {
            return new RespuestaGeneral<object>
            {
                Error = true,
                Mensaje = "El evento ya alcanzó su capacidad máxima.",
                Resultado = null
            };
        }

        // Registrar la inscripción
        Inscripcion nuevaInscripcion = new Inscripcion
        {
            UsuarioId = usuarioId,
            EventoId = eventoId,
            FechaInscripcion = DateTime.UtcNow
        };

        await _inscripcionRepository.AddAsync(nuevaInscripcion);

        // Actualizar el número de asistentes registrados
        evento.AsistentesRegistrados += 1;
        await _eventRepository.UpdateAsync(evento);

        return new RespuestaGeneral<object>
        {
            Error = false,
            Mensaje = "Inscripción realizada con éxito.",
            Resultado = null
        };
    }

    public async Task<IEnumerable<UsuarioInscritoDto>> ObtenerUsuariosInscritosAsync(int eventoId)
    {
        List<Usuario> usuarios = await _inscripcionRepository.GetUsuariosInscritosAsync(eventoId);

        // Transformar a DTO
        return usuarios.Select(u => new UsuarioInscritoDto
        {
            Id = u.Id,
            Nombre = u.Nombre,
            Correo = u.Correo
        });
    }


}
