using EventsApi.Domain.Entities;
using EventsApi.Models;
using EventsApi.Repositorio;

namespace EventsApi.Repositorio
{
    public interface IUsuarioRepository : IGenericRepository<Usuario>
    {
        Task<Usuario?> GetByEmailAsync(string email);
        Task<Usuario?> GetUsuarioByIdAsync(int userId);
        Task<IEnumerable<Usuario>> ObtenerTodosLosUsuariosAsync();

    }
}
