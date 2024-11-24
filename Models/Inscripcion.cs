using System;
using System.ComponentModel.DataAnnotations;

namespace EventsApi.Models
{
    public class Inscripcion
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; } = null!;

        public int EventoId { get; set; }
        public Evento? Evento { get; set; } = null!;

        public DateTime FechaInscripcion { get; set; } = DateTime.UtcNow; 
    }

}
