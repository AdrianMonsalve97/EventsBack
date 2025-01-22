using EventsApi.Domain.Enums;

namespace EventsApi.Domain.Entities;

public class Empresa
{
    public string NombreEmpresa { get; set; } = null!;
    public string IndentificacionEmpresa { get; set; } = null!;
    public TipoDocumento TipoDocumento { get; set; } = TipoDocumento.NIT;
    public string NombreContactoEmpresa { get; set; } = null!;
    public string NumeroContatoEmpresa { get; set; } = null!;
    
}