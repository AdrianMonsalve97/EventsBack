namespace EventsApi.Models.DTO
{

    public class InscripcionDto
    {
        public int EventoId { get; set; }
        public string? EventoNombre { get; set; }
        public DateTime FechaInscripcion { get; set; }
    }
}
