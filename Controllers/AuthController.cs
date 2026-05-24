using Microsoft.AspNetCore.Mvc;
using ProyectoDonacion.DTOs;
using ProyectoDonacion.Services;

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

        [HttpPost()]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            try
            {
                var user = await _authService.Register(dto);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
