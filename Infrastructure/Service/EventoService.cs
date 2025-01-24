using AutoMapper;
using EventsApi.Domain.Entities;
using EventsApi.Models;
using EventsApi.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Models.resgeneral;

namespace EventsApi.Services;

public class EventoService
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;

    public EventoService(IEventRepository eventRepository, IMapper mapper)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
    }
    public async Task<RespuestaGeneral<object>> CreateEventAsync(Evento evento, int userId)
    {
        // Validar capacidad máxima
        if (evento.CapacidadMaxima > 40)
        {
            return new RespuestaGeneral<object>
            {
                Error = true,
                Mensaje = "La capacidad máxima no puede superar los 40 asistentes.",
                Resultado = null
            };
        }

        // Buscar al usuario en la base de datos
        Usuario? usuario = await _eventRepository.GetUsuarioByIdAsync(userId); // Método en repositorio para obtener usuario
        if (usuario == null)
        {
            return new RespuestaGeneral<object>
            {
                Error = true,
                Mensaje = "Usuario no encontrado.",
                Resultado = null
            };
        }

        // Asignar datos al evento
        evento.UsuarioCreadorId = userId;
        evento.UsuarioCreadorNombre = usuario.Nombre; // Asignar el nombre del usuario
        evento.AsistentesRegistrados = 0;

        // Guardar en la base de datos
        await _eventRepository.AddAsync(evento);

        return new RespuestaGeneral<object>
        {
            Error = false,
            Mensaje = "Evento creado con éxito.",
            Resultado = evento.Id // Retornar el ID del evento creado
        };
    }


    public async Task<RespuestaGeneral<object>> UpdateEventAsync(int eventId, Evento updatedEvent, int userId)
    {
        Evento? evento = await _eventRepository.GetByIdAsync(eventId);
        if (evento == null)
        {
            return new RespuestaGeneral<object>
            {
                Error = true,
                Mensaje = "El evento no existe.",
                Resultado = null
            };
        }

        if (evento.UsuarioCreadorId != userId)
        {
            return new RespuestaGeneral<object>
            {
                Error = true,
                Mensaje = "Solo el creador del evento puede editarlo.",
                Resultado = null
            };
        }

        evento.FechaHora = updatedEvent.FechaHora;
        evento.Ubicacion = updatedEvent.Ubicacion;
        evento.CapacidadMaxima = updatedEvent.CapacidadMaxima;

        await _eventRepository.UpdateAsync(evento);

        return new RespuestaGeneral<object>
        {
            Error = false,
            Mensaje = "Evento actualizado con éxito.",
            Resultado = null
        };
    }


    public async Task<RespuestaGeneral<object>> DeleteEventAsync(int eventId)
    {
        bool canDelete = await _eventRepository.CanDeleteEventAsync(eventId);
        if (!canDelete)
        {
            return new RespuestaGeneral<object>
            {
                Error = true,
                Mensaje = "No se puede eliminar el evento porque tiene asistentes inscritos.",
                Resultado = null
            };
        }
        Evento? evento = await _eventRepository.GetByIdAsync(eventId);
        if (evento == null)
        {
            return new RespuestaGeneral<object>
            {
                Error = true,
                Mensaje = "El evento no existe.",
                Resultado = null
            };
        }
        await _eventRepository.DeleteAsync(evento);
        return new RespuestaGeneral<object>
        {
            Error = false,
            Mensaje = "Evento eliminado con éxito.",
            Resultado = null
        };
    }
    public async Task<RespuestaGeneral<IEnumerable<Evento>>> GetEventsByDateAsync(DateTime date)
    {
        IEnumerable<Evento> events = await _eventRepository.GetAvailableEventsAsync();

        List<Evento> filteredEvents = events
            .Where(e => e.FechaHora.Value >= date.Date)
            .ToList();

        RespuestaGeneral<IEnumerable<Evento>> response = new RespuestaGeneral<IEnumerable<Evento>>
        {
            Error = false,
            Mensaje = "Eventos obtenidos con éxito.",
            Resultado = filteredEvents
        };

        return response;
    }

    public async Task<IEnumerable<EventoDto>> GetFilteredEventsAsync(EventoFiltroDto filtro)
    {
        IEnumerable<Evento> eventos = await _eventRepository.GetFilteredEventsAsync(filtro);
        IEnumerable <EventoDto> eventosDto = _mapper.Map<IEnumerable<EventoDto>>(eventos);

        return eventosDto;
    }

    public async Task<Evento?> GetByIdAsync(int eventoId)
    {
        return await _eventRepository.GetByIdAsync(eventoId);
    }

    public async Task<EventoDto?> ObtenerEventoConDetallesAsync(int eventoId)
    {
        Evento? evento = await _eventRepository
            .GetEventoConInscripcionesAsync(eventoId) ?? throw new KeyNotFoundException("El evento no existe");

        return _mapper.Map<EventoDto>(evento);
    }


}