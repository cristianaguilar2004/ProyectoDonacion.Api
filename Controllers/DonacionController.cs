using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoDonacion.Common;
using ProyectoDonacion.Services.Donaciones;

namespace ProyectoDonacion.Controllers
{
    [Route("donaciones")]
    [ApiController, Authorize()]
    public class DonacionController : ControllerBase
    {
        private readonly DonacionService _donacionService;
        public DonacionController(DonacionService donacionService)
        {
            _donacionService = donacionService;
        }


        [HttpGet]
        public async Task<IActionResult> GetDonaciones([FromQuery] bool soloActivos = true)
        {
            var response = await _donacionService.GetDonaciones(soloActivos);
            if (response.Type != ResponseType.Ok)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> PostDonacion([FromForm] IFormCollection form)
        {
            var response = await _donacionService.CreateDonacion(form);
            if (response.Type != ResponseType.Ok)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
