using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoDonacion.Common;
using ProyectoDonacion.DTOs.Sucursales;
using ProyectoDonacion.Services.Sucursales;

namespace ProyectoDonacion.Controllers
{
    [Route("sucursales")]
    [ApiController, Authorize]
    public class SucursalController : ControllerBase
    {
        private readonly SucursalService _sucursalService;
        public SucursalController(SucursalService sucursalService)
        {
            _sucursalService = sucursalService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllSucursales(bool soloActivos = true)
        {
            var response = await _sucursalService.GetAllSucursales(soloActivos);

            if(response.Type != ResponseType.Ok)
                return BadRequest(response);

            return Ok(response);
        }



        [HttpPost]
        public async Task<IActionResult> CreateSucursal([FromBody] CreateSucursalDto dto)
        {
            var response = await _sucursalService.CreateSucursal(dto);

            if (response.Type != ResponseType.Ok)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
