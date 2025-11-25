using lecheriaSC.Core.DTOs;
using lecheriaSC.Core.Entidades;
using lecheriaSC.Core.Interfaces;
using lecheriaSC.Core.Mapeadores;
using Microsoft.AspNetCore.Mvc;

namespace lecheriaSC.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SucursalController : ControllerBase
    {
        private readonly IRepositorioSucursal _repositorio;
        private readonly ILogger<SucursalController> _logger;

        public SucursalController(IRepositorioSucursal repositorio, ILogger<SucursalController> logger)
        {
            _repositorio = repositorio;
            _logger = logger;
        }

        [HttpGet("todos")]
        public async Task<ActionResult<IEnumerable<SucursalDTO>>> ObtenerTodos()
        {
            try
            {
                var sucursales = await _repositorio.ObtenerTodosAsync();
                return Ok(sucursales.Select(MapeadorSucursal.EntidadADTO));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("{codigo}")]
        public async Task<ActionResult<SucursalDTO>> ObtenerPorCodigo(string codigo)
        {
            try
            {
                var sucursal = await _repositorio.ObtenerPorCodigoAsync(codigo);
                if (sucursal == null) return NotFound("Sucursal no encontrada");

                return Ok(MapeadorSucursal.EntidadADTO(sucursal));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPost("crear")]
        public async Task<ActionResult<SucursalDTO>> Crear([FromBody] SucursalDTO dto)
        {
            try
            {
                var sucursal = MapeadorSucursal.DTOAEntidad(dto);
                await _repositorio.CrearAsync(sucursal);
                return CreatedAtAction(nameof(ObtenerPorCodigo), new { codigo = sucursal.Codigo }, MapeadorSucursal.EntidadADTO(sucursal));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPut("actualizar/{id}")]
        public async Task<ActionResult> Actualizar(int id, [FromBody] SucursalDTO dto)
        {
            try
            {
                var sucursal = await _repositorio.ObtenerPorIdAsync(id);
                if (sucursal == null) return NotFound("Sucursal no encontrada");

                sucursal.Nombre = dto.Nombre;
                sucursal.Gerente = dto.Gerente;
                sucursal.EmpleadosActivos = dto.EmpleadosActivos;
                sucursal.SaldoPresupuesto = dto.SaldoPresupuesto;
                sucursal.TotalPresupuesto = dto.TotalPresupuesto;
                sucursal.Activa = dto.Activa;

                await _repositorio.ActualizarAsync(sucursal);
                return Ok("Sucursal actualizada");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}
