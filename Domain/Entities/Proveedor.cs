using System.ComponentModel.DataAnnotations;
using EventsApi.Domain.Enums;

namespace EventsApi.Domain.Entities

{
    public class Proveedor
    {
        [Required (ErrorMessage = "El número de identificacion del proveedor es obligatorio")]
        public string IdentificacionProveedor { get; set; } = null!;
        [Required (ErrorMessage = "El nombre del proveedor es obligatorio")]
        public string NombreProveedor { get; set; } = null!;
        [Required (ErrorMessage = "El Tipo de documento del proveedor es obligatorio")]
        public TipoDocumento TipoDocumento { get; set; } = TipoDocumento.NIT;
    }
}

