namespace EventsApi.Application.DTO
{
    public class LoginResponseDto
    {
        public string Token { get; set; }
        public int UsuarioId { get; set; }
        public string UsuarioNombre { get; set; }
    }
}
