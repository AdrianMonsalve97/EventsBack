using System.ComponentModel.DataAnnotations;

namespace EventsApi.Models
{
    public class LoginDto
    {
        [Required]
        public string CorreoCorporativo { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}
