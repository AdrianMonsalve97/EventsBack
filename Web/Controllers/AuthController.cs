using Microsoft.AspNetCore.Mvc;
using EventsApi.Services;
using EventsApi.Models;
using Models.resgeneral;

namespace EventsApi.Controllers
{
    [ApiController]
    [Route("4uth/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Usuario usuario)
        {
            RespuestaGeneral<object> response = await _authService.RegisterAsync(usuario);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            RespuestaGeneral<string> response = await _authService.LoginAsync(loginDto);
            return Ok(response);
        }
    }
}
