using EventsApi.Domain.Entities;

namespace EventsApi.Models.DTO
{
    public class EventoDto
    {
        public string? Nombre { get; set; }
        public string? UsuarioCreadorNombre { get; set; }
        public string? Ubicacion { get; set; }
        public int? CapacidadMaxima { get; set; }
        public string? Descripcion { get; set; }
        public int? AsistentesRegistrados { get; set; }
        public DateTime? FechaHora { get; set; }
        public string? NumeroEvento { get; set; }
        public List<UsuarioInscritoDto>? Inscripciones { get; set; } = new List<UsuarioInscritoDto>();
        public int Id { get; internal set; }
    }
}
