using EventsApi.Models.DTO;

namespace EventsApi.Application.DTO;

public class UsuarioDto
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string CorreoCorporativo { get; set; }
    public string CorreoPersonal { get; set; }
    public string Rol { get; set; }
    public string TipoDocumento { get; set; } 
    public long DocumentoIdentidad { get; set; }
    public long CelularPersonal { get; set; }
    public long CelularCorporativo { get; set; }
    public DateTime? FechaContratoInicio { get; set; }
    public DateTime? FechaContratoFin { get; set; }
    public string? NombreEmpresa { get; set; }

}