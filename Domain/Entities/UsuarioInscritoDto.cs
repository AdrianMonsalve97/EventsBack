namespace EventsApi.Domain.Entities
{
    public class UsuarioInscritoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
    }

}
