using EventsApi.Application.DTO;
using EventsApi.Domain.Entities;
using EventsApi.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using EventsApi.Services;
using EventsApi.Models;
using Models.resgeneral;

namespace EventsApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UsuarioDto usuarioDto)
        {
            RespuestaGeneral<object> result = await _authService.Register(usuarioDto);
            return Ok(result);  
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            RespuestaGeneral<LoginResponseDto> response = await _authService.LoginAsync(loginDto);
    
            if (response.Error)
            {
                return BadRequest(response); 
            }

            return Ok(response); 
        }

    }
}
