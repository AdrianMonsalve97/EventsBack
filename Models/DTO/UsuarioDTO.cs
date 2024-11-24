using EventsApi.Models;
public class UsuarioDto
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Correo { get; set; }
    public string Rol { get; set; }
    public List<EventoDto> EventosCreados { get; set; } = new List<EventoDto>();
    public List<InscripcionDto> Inscripciones { get; set; } = new List<InscripcionDto>();
}



public class InscripcionDto
{
    public int EventoId { get; set; }
    public string EventoNombre { get; set; }
    public DateTime FechaInscripcion { get; set; }
}