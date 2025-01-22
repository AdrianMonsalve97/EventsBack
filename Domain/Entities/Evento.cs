using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using EventsApi.Models.Enums;

namespace EventsApi.Domain.Entities
{
    public class Evento
    {
        public int Id { get; set; }

        [MaxLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres.")]
        public string Descripcion { get; set; } = string.Empty;

        public DateTime? FechaHora { get; set; }

        [MaxLength(200, ErrorMessage = "La ubicación no puede exceder los 200 caracteres.")]
        public string Ubicacion { get; set; } = string.Empty;

        [Range(1, 40, ErrorMessage = "La capacidad debe estar entre 1 y 40 asistentes.")]
        public int CapacidadMaxima { get; set; }

        public int AsistentesRegistrados { get; set; } = 0;

        [Required(ErrorMessage = "La prioridad es obligatoria.")]
        public Prioridad Prioridad { get; set; } = Prioridad.NoUrgente;

        // Fechas agrupadas
        public FechasEventos Fechas { get; set; } = new FechasEventos();

        // Relaciones
        public int UsuarioCreadorId { get; set; }

        [JsonIgnore]
        public Usuario? UsuarioCreador { get; set; }

        public string? UsuarioCreadorNombre { get; set; }

        public ICollection<Inscripcion> Inscripciones { get; set; } = new List<Inscripcion>();

        // Métodos de Negocio
        public bool PuedeEliminarse() => AsistentesRegistrados == 0;

        public bool TieneCupo() => AsistentesRegistrados < CapacidadMaxima;
    }
    
    public class FechasEventos
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime FechaAsignacion { get; set; }
        public DateTime FechaCotizacion { get; set; }
        public DateTime FechaAprovacion { get; set; }

        // Formateo de fechas
        public string FechaInicioFormato => FechaInicio.ToString("yyyy-MM-dd HH:mm:ss");
        public string FechaFinFormato => FechaFin.ToString("yyyy-MM-dd HH:mm:ss");
        public string FechaAsignacionFormato => FechaAsignacion.ToString("yyyy-MM-dd HH:mm:ss");
        public string FechaCotizacionFormato => FechaCotizacion.ToString("yyyy-MM-dd HH:mm:ss");
        public string FechaAprovacionFormato => FechaAprovacion.ToString("yyyy-MM-dd HH:mm:ss");
    }
}

