using EventsApi.Models;
using EventsApi.Models.DTO;
public class UsuarioDto
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Correo { get; set; }
    public string Rol { get; set; }
    public List<EventoDto> EventosCreados { get; set; } = new List<EventoDto>();
    public List<InscripcionDto> Inscripciones { get; set; } = new List<InscripcionDto>();
}


