using System.ComponentModel.DataAnnotations;
using EventsApi.Domain.Enums;
using EventsApi.Models.DTO;

namespace EventsApi.Application.DTO;

public class UsuarioDto
{
    public int Id { get; set; }
    [Required, MaxLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
    public string Nombre { get; set; }
    [EmailAddress(ErrorMessage = "El correo debe tener un formato válido.")]
    public string CorreoCorporativo { get; set; }
    
    public string PasswordHash { get; set; } 

    [EmailAddress(ErrorMessage = "El correo debe tener un formato válido.")]

    public string CorreoPersonal { get; set; }
    [Required(ErrorMessage = "El rol es obligatorio.")]

    public Rol Rol { get; set; } = Rol.Usuario;
    [Required(ErrorMessage = "El tipo documento es obligatorio.")]
    public TipoDocumento TipoDocumento { get; set; } = TipoDocumento.CC;
    [Required, Range(1, 999999999999999, ErrorMessage = "El número de identificación no puede exceder los 15 dígitos.")]
    public long DocumentoIdentidad { get; set; }
    public long CelularPersonal { get; set; }
    public long CelularCorporativo { get; set; }
    public DateTime? FechaContratoInicio { get; set; }
    public DateTime? FechaContratoFin { get; set; }
    public string? NombreEmpresa { get; set; }

}