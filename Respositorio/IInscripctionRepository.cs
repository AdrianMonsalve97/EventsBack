using EventsApi.Models;
using EventsApi.Repositorio;
public interface IInscriptionRepository : IGenericRepository<Inscripcion>
{
    Task<bool> ExisteInscripcionAsync(int usuarioId, int eventoId);
    Task<int> ContarInscritosAsync(int eventoId);
    Task<IEnumerable<Inscripcion>> ObtenerPorUsuarioAsync(int usuarioId);
    Task<IEnumerable<Inscripcion>> ObtenerPorEventoAsync(int eventoId);
    Task<List<Usuario>> GetUsuariosInscritosAsync(int eventoId);
    Task<int> CountUserInscripcionesAsync(int usuarioId);
}
