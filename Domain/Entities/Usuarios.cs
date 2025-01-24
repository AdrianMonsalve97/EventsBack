using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EventsApi.Domain.Enums;

namespace EventsApi.Domain.Entities
{
    public class Usuario : IValidatableObject
    {
        [Required]
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;

        [Required, CelularValidation]
        public long CelularPersonal { get; set; }

        [Required, CelularValidation]
        public long CelularCorporativo { get; set; }

        [Required(ErrorMessage = "El tipo documento es obligatorio.")]
        public TipoDocumento TipoDocumento { get; set; } = TipoDocumento.CC;

        [Required, Range(1, 999999999999999, ErrorMessage = "El número de identificación no puede exceder los 15 dígitos.")]
        public long DocumentoIdentidad { get; set; }

        [EmailAddress(ErrorMessage = "El correo debe tener un formato válido.")]
        public string? CorreoCorporativo { get; set; }

        [EmailAddress(ErrorMessage = "El correo debe tener un formato válido.")]
        public string? CorreoPersonal { get; set; }

        public string PasswordHash { get; set; } = null!;

        public string? PasswordSalt { get; set; } 

        [Required(ErrorMessage = "El rol es obligatorio.")]
        public Rol Rol { get; set; } = Rol.Usuario;

        public DateTime? FechaContratoInicio { get; set; }
        public DateTime? FechaContratoFin { get; set; }

        public Empresa? Empresa { get; set; }

        public ICollection<Evento> EventosCreados { get; set; } = new List<Evento>();
        public ICollection<Inscripcion> Inscripciones { get; set; } = new List<Inscripcion>();

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;

        // Validaciones personalizadas
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Validar que al menos uno de los correos sea obligatorio
            if (string.IsNullOrEmpty(CorreoCorporativo) && string.IsNullOrEmpty(CorreoPersonal))
            {
                yield return new ValidationResult(
                    "Debe proporcionar al menos un correo (Corporativo o Personal).",
                    new[] { nameof(CorreoCorporativo), nameof(CorreoPersonal) }
                );
            }

            // Validar que la fecha de inicio sea anterior a la fecha de fin
            if (FechaContratoInicio.HasValue && FechaContratoFin.HasValue && FechaContratoInicio > FechaContratoFin)
            {
                yield return new ValidationResult(
                    "La fecha de inicio del contrato debe ser anterior a la fecha de fin.",
                    new[] { nameof(FechaContratoInicio), nameof(FechaContratoFin) }
                );
            }
        }
    }

    // Validación personalizada para celular
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CelularValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not long celular || celular < 3000000000 || celular > 3999999999)
            {
                return new ValidationResult("El número de celular debe tener exactamente 10 dígitos.");
            }
            return ValidationResult.Success;
        }
    }
}
