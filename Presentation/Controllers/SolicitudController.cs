using lecheriaSC.Core.DTOs;
using lecheriaSC.Core.Interfaces;
using lecheriaSC.Core.Mapeadores;
using lecheriaSC.Consumos;
using Microsoft.AspNetCore.Mvc;

namespace lecheriaSC.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SolicitudController : ControllerBase
    {
        private readonly IRepositorioSolicitud _repositorio;
        private readonly ContabilidadService _contabilidadService;
        private readonly ILogger<SolicitudController> _logger;

        public SolicitudController(
            IRepositorioSolicitud repositorio,
            ContabilidadService contabilidadService,
            ILogger<SolicitudController> logger)
        {
            _repositorio = repositorio;
            _contabilidadService = contabilidadService;
            _logger = logger;
        }

        [HttpGet("pendientes/{codigoSucursal}")]
        public async Task<ActionResult<IEnumerable<SolicitudResponseDTO>>> ObtenerPendientes(string codigoSucursal)
        {
            try
            {
                var solicitudes = await _repositorio.ObtenerPendientesPorSucursalAsync(codigoSucursal);
                return Ok(solicitudes.Select(MapeadorSolicitud.EntidadADTO));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                return StatusCode(500, "Error interno");
            }
        }

        [HttpPost("crear")]
        public async Task<ActionResult<SolicitudResponseDTO>> Crear([FromBody] CrearSolicitudDTO dto)
        {
            try
            {
                if (dto.Monto > 0 && string.IsNullOrEmpty(dto.CodigoSucursal))
                {
                    return BadRequest("Código de sucursal requerido");
                }

                var solicitud = MapeadorSolicitud.DTOAEntidad(dto);
                await _repositorio.CrearAsync(solicitud);
                return CreatedAtAction(nameof(ObtenerPendientes), new { codigoSucursal = dto.CodigoSucursal }, MapeadorSolicitud.EntidadADTO(solicitud));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                return StatusCode(500, "Error interno");
            }
        }

        [HttpPut("aprobar/{id}")]
        public async Task<ActionResult> Aprobar(int id, [FromBody] AprobarSolicitudDTO dto)
        {
            try
            {
                var solicitud = await _repositorio.ObtenerPorIdAsync(id);
                if (solicitud == null) return NotFound("Solicitud no encontrada");

                if (dto.Aprobar && solicitud.Monto > 0)
                {
                    await _contabilidadService.AprobarGastoAsync(id, solicitud.Monto);
                }

                var estado = dto.Aprobar ? "Aprobada" : "Rechazada";
                await _repositorio.ActualizarEstadoAsync(id, estado, dto.Observaciones);
                return Ok($"Solicitud {estado.ToLower()}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                return StatusCode(500, "Error interno");
            }
        }
    }
}

