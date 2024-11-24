using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventsApi.Models
{
    public class Usuario
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; } = null!;
        [Required]
        public string Correo { get; set; } = null!;
        [Required]
        public string PasswordHash { get; set; } = null!; // Contraseña encriptada
        [Required]
        public string Rol { get; set; } = "Usuario"; 

        // Relaciones
        public ICollection<Evento> EventosCreados { get; set; } = new List<Evento>();
        public ICollection<Inscripcion> Inscripciones { get; set; } = new List<Inscripcion>();
    }

    
}


