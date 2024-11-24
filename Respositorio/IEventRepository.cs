using EventsApi.Models;
using EventsApi.Repositorio;

public interface IEventRepository : IGenericRepository<Evento>
{
    Task<IEnumerable<Evento>> GetAvailableEventsAsync(); // Eventos disponibles
    Task<int> GetRegisteredAttendeesAsync(int eventId); // Inscritos en un evento
    Task<bool> CanDeleteEventAsync(int eventId); // Validar eliminación

    IQueryable<Evento> BuildQuery(EventoFiltroDto filtro); // Generar consulta con filtros
    Task<IEnumerable<Evento>> GetFilteredEventsAsync(EventoFiltroDto filtro); // Obtener eventos filtrados
    Task<Usuario?> GetUsuarioByIdAsync(int userId);
    Task<Evento?> GetEventoConInscripcionesAsync(int eventoId);


}
