namespace EventsApi.Models.DTO
{
    public class EventoFiltroDto
    {
        public string? Nombre { get; set; }
        public string? Ubicacion { get; set; }
        public DateTime? FechaHora { get; set; }
        public int? CapacidadMaxima { get; set; }
    }

}
