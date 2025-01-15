using EventsApi.Data;
using EventsApi.Domain.Entities;
using EventsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EventsApi.Repositorio
{
    public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(AppDbContext context) : base(context)
        {
        }

        // Método específico para buscar por correo
        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == email);
        }

        public async Task<Usuario?> GetUsuarioByIdAsync(int userId)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<Usuario?> GetUsuarioConEventosAsync(int userId)
        {
            return await _context.Usuarios
                .Include(u => u.EventosCreados)
                .Include(u => u.Inscripciones)
                    .ThenInclude(i => i.Evento)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }public async Task<IEnumerable<Usuario>> ObtenerTodosLosUsuariosAsync()
    {
        return await _context.Usuarios
            .Include(u => u.EventosCreados)
            .Include(u => u.Inscripciones)
                .ThenInclude(i => i.Evento) // Incluye detalles del evento en las inscripciones
            .ToListAsync();
    }



    }
}
