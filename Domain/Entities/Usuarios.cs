using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using EventsApi.Models.Enums;

namespace EventsApi.Domain.Entities
{
    public class Usuario
    {
        [Required]
        public int Id { get; set; }

        [Required, MaxLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string Nombre { get; set; } = null!;

        [Required, EmailAddress(ErrorMessage = "El correo debe tener un formato válido.")]
        public string Correo { get; set; } = null!;

        [Required, MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        public string PasswordHash { get; set; } = null!; // Contraseña encriptada

        [Required(ErrorMessage = "El rol es obligatorio.")]
        public Rol Rol { get; set; } = Rol.Usuario; // Uso del enum Rol con valor predeterminado

        // Relaciones
        public ICollection<Evento> EventosCreados { get; set; } = new List<Evento>();
        public ICollection<Inscripcion> Inscripciones { get; set; } = new List<Inscripcion>();
    }
}