using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EventsApi.Domain.Enums;

namespace EventsApi.Domain.Entities
{
    public class Usuario
    {
        [Required]
        public int Id { get; set; }

        [Required, MaxLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string Nombre { get; set; } = null!;

        [Required, Range(1000000000, 9999999999, ErrorMessage = "El número de celular debe tener exactamente 10 dígitos.")]
        public long CelularPersonal { get; set; }

        [Required, Range(1000000000, 9999999999, ErrorMessage = "El número de celular debe tener exactamente 10 dígitos.")]
        public long CelularCorporativo { get; set; }

        [Required(ErrorMessage = "El tipo documento es obligatorio.")]
        public TipoDocumento TipoDocumento { get; set; } = TipoDocumento.CC;

        [Required, Range(1, 999999999999999, ErrorMessage = "El número de identificación no puede exceder los 15 dígitos.")]
        public long DocumentoIdentidad { get; set; }

        [Required, EmailAddress(ErrorMessage = "El correo debe tener un formato válido.")]
        public string CorreoCorporativo { get; set; } = null!;

        [Required, EmailAddress(ErrorMessage = "El correo debe tener un formato válido.")]
        public string CorreoPersonal { get; set; } = null!;

        [Required, MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        public string PasswordHash { get; set; } = null!; // Contraseña encriptada

        [Required(ErrorMessage = "El rol es obligatorio.")]
        public Rol Rol { get; set; } = Rol.Usuario;

        // Fechas asociadas al usuario
        public DateTime? FechaContratoInicio { get; set; }
        public DateTime? FechaContratoFin { get; set; }

        public Empresa? Empresa { get; set; }

        // Relaciones
        public ICollection<Evento> EventosCreados { get; set; } = new List<Evento>();
        public ICollection<Inscripcion> Inscripciones { get; set; } = new List<Inscripcion>();
    }
}
