using EventsApi.Data;
using EventsApi.Models;
using EventsApi.Repositorio;
using Microsoft.EntityFrameworkCore;

public class EventRepository : GenericRepository<Evento>, IEventRepository
{
    public EventRepository(AppDbContext context) : base(context) { }

    // Construir consulta basada en filtros
    public IQueryable<Evento> BuildQuery(EventoFiltroDto filtro)
    {
        IQueryable<Evento> query = _context.Eventos;

        if (!string.IsNullOrEmpty(filtro.Nombre))
        {
            query = query.Where(e => e.Nombre.Contains(filtro.Nombre));
        }

        if (!string.IsNullOrEmpty(filtro.Ubicacion))
        {
            query = query.Where(e => e.Ubicacion.Contains(filtro.Ubicacion));
        }

        if (filtro.FechaHora.HasValue)
        {
            DateTime fechaInicio = filtro.FechaHora.Value.Date;
            DateTime fechaFin = fechaInicio.AddDays(1);

            query = query.Where(e => e.FechaHora >= fechaInicio && e.FechaHora < fechaFin);
        }

        if (filtro.CapacidadMaxima.HasValue && filtro.CapacidadMaxima > 0)
        {
            query = query.Where(e => e.CapacidadMaxima >= filtro.CapacidadMaxima.Value);
        }

        return query;
    }



    // Obtener eventos disponibles
    public async Task<IEnumerable<Evento>> GetAvailableEventsAsync()
    {
        return await _context.Eventos
            .Include(e => e.Inscripciones)
            .Where(e => e.FechaHora >= DateTime.Now)
            .ToListAsync();
    }

    // Contar asistentes registrados en un evento
    public async Task<int> GetRegisteredAttendeesAsync(int eventId)
    {
        return await _context.Inscripciones
            .CountAsync(i => i.EventoId == eventId);
    }

    // Validar si un evento puede eliminarse
    public async Task<bool> CanDeleteEventAsync(int eventId)
    {
        int attendees = await GetRegisteredAttendeesAsync(eventId);
        return attendees == 0;
    }

    // Obtener eventos filtrados
    public async Task<IEnumerable<Evento>> GetFilteredEventsAsync(EventoFiltroDto filtro)
    {
        IQueryable<Evento> query = BuildQuery(filtro);
        return await query.ToListAsync();
    }


    public async Task<Usuario?> GetUsuarioByIdAsync(int userId)
    {
        return await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == userId);
    }
    public async Task<Evento?> GetEventoConInscripcionesAsync(int eventoId)
    {
        return await _context.Eventos
            .Include(e => e.Inscripciones)
                .ThenInclude(i => i.Usuario)
            .FirstOrDefaultAsync(e => e.Id == eventoId);
    }


}
