using EventsApi.Data;
using EventsApi.Domain.Entities;
using EventsApi.Models;
using EventsApi.Repositorio;
using Microsoft.EntityFrameworkCore;

public class InscripcionRepository : GenericRepository<Inscripcion>, IInscriptionRepository
{
    public InscripcionRepository(AppDbContext context) : base(context) { }

    public async Task<bool> ExisteInscripcionAsync(int usuarioId, int eventoId)
    {
        return await _context.Inscripciones.AnyAsync(i => i.UsuarioId == usuarioId && i.EventoId == eventoId);
    }

    public async Task<int> ContarInscritosAsync(int eventoId)
    {
        return await _context.Inscripciones.CountAsync(i => i.EventoId == eventoId);
    }

    public async Task<IEnumerable<Inscripcion>> ObtenerPorUsuarioAsync(int usuarioId)
    {
        List<Inscripcion> inscripciones = await _context.Inscripciones
            .Include(i => i.Evento)
            .Where(i => i.UsuarioId == usuarioId)
            .ToListAsync();

        return inscripciones;
    }

    public async Task<IEnumerable<Inscripcion>> ObtenerPorEventoAsync(int eventoId)
    {
        List<Inscripcion> inscripciones = await _context.Inscripciones
            .Include(i => i.Usuario)
            .Where(i => i.EventoId == eventoId)
            .ToListAsync();

        return inscripciones;
    }

    public async Task<List<Usuario>> GetUsuariosInscritosAsync(int eventoId)
    {
        List<Usuario> usuariosInscritos = await _context.Inscripciones
            .Where(i => i.EventoId == eventoId)
            .Include(i => i.Usuario)
            .Select(i => i.Usuario)
            .ToListAsync();

        return usuariosInscritos;
    }

    public async Task<int> CountUserInscripcionesAsync(int usuarioId)
    {
        int inscripciones = await _context.Inscripciones
            .CountAsync(i => i.UsuarioId == usuarioId);
    
        return inscripciones;
    }

    public async Task<IEnumerable<Inscripcion>> GetInscripcionesByUsuarioAsync(int usuarioId)
    {
        List<Inscripcion> inscripciones = await _context.Inscripciones
            .Where(i => i.UsuarioId == usuarioId)
            .Include(i => i.Evento) 
            .ToListAsync();

        return inscripciones;
    }

}

