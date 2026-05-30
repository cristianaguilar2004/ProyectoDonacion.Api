using Microsoft.AspNetCore.Mvc;
using ProyectoDonacion.Common;
using ProyectoDonacion.DTOs;
using ProyectoDonacion.DTOs.Auth;
using ProyectoDonacion.Services.Auth;

namespace ProyectoDonacion.Controllers
{
    [Route("users")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _authService.GetAllUsers();

            if (response.Type != ResponseType.Ok)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById([FromRoute] string id)
        {
            var response = await _authService.GetUserById(id);

            if (response.Type != ResponseType.Ok)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var response = await _authService.Login(dto);

            if (response.Type != ResponseType.Ok)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPost()]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var response = await _authService.Register(dto);

            if (response.Type != ResponseType.Ok)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> DesactivarUsuario([FromRoute] string id)
        {
            var response = await _authService.DesactivarUsuario(id);
            if (response.Type != ResponseType.Ok)
                return BadRequest(response);
            return Ok(response);


        }
    }
}
