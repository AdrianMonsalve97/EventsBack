using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EventsApi.Models
{
    public class Evento
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Nombre { get; set; } = null!;
        [MaxLength(500)]
        public string Descripcion { get; set; } = null!;
        public DateTime? FechaHora { get; set; }
        [MaxLength(200)]
        public string Ubicacion { get; set; } = null!;
        [Range(1, 40, ErrorMessage = "La capacidad debe estar entre 1 y 40 asistentes.")]
        public int CapacidadMaxima { get; set; }
        public int AsistentesRegistrados { get; set; } = 0;
        // Relaciones
        public int UsuarioCreadorId { get; set; }
        [JsonIgnore] // Ignorar esta propiedad en la solicitud
        public Usuario? UsuarioCreador { get; set; }
        public string? UsuarioCreadorNombre { get; set; }
        public ICollection<Inscripcion> Inscripciones { get; set; } = new List<Inscripcion>();
        // Métodos de Negocio
        public bool PuedeEliminarse() => AsistentesRegistrados == 0;
        public bool TieneCupo() => AsistentesRegistrados < CapacidadMaxima;
    }




}

