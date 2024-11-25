using EventsApi.Models;
using EventsApi.Repositorio;

public interface IEventRepository : IGenericRepository<Evento>
{
    Task<IEnumerable<Evento>> GetAvailableEventsAsync(); 
    Task<int> GetRegisteredAttendeesAsync(int eventId); 
    Task<bool> CanDeleteEventAsync(int eventId);

    IQueryable<Evento> BuildQuery(EventoFiltroDto filtro); 
    Task<IEnumerable<Evento>> GetFilteredEventsAsync(EventoFiltroDto filtro);
    Task<Usuario?> GetUsuarioByIdAsync(int userId);
    Task<Evento?> GetEventoConInscripcionesAsync(int eventoId);


}
