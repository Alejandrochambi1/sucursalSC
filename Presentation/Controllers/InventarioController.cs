using lecheriaSC.Core.DTOs;
using lecheriaSC.Core.Entidades;
using lecheriaSC.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace lecheriaSC.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventarioController : ControllerBase
    {
        private readonly IRepositorioInventario _repositorio;
        private readonly IRepositorioSolicitud _repositorioSolicitud;
        private readonly ILogger<InventarioController> _logger;

        public InventarioController(
            IRepositorioInventario repositorio,
            IRepositorioSolicitud repositorioSolicitud,
            ILogger<InventarioController> logger)
        {
            _repositorio = repositorio;
            _repositorioSolicitud = repositorioSolicitud;
            _logger = logger;
        }

        [HttpGet("sucursal/{codigoSucursal}")]
        public async Task<ActionResult<IEnumerable<InventarioDTO>>> ObtenerPorSucursal(string codigoSucursal)
        {
            try
            {
                var inventarios = await _repositorio.ObtenerPorSucursalAsync(codigoSucursal);
                return Ok(inventarios.Select(i => new InventarioDTO
                {
                    Id = i.Id,
                    ProductoId = i.ProductoId,
                    NombreProducto = i.NombreProducto,
                    Cantidad = i.Cantidad,
                    StockMinimo = i.StockMinimo,
                    PrecioUnitario = i.PrecioUnitario,
                    EnAlerta = i.EnAlerta
                }));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                return StatusCode(500, "Error interno");
            }
        }

        [HttpGet("alertas/{codigoSucursal}")]
        public async Task<ActionResult<IEnumerable<InventarioDTO>>> ObtenerEnAlerta(string codigoSucursal)
        {
            try
            {
                var inventarios = await _repositorio.ObtenerEnAlertaAsync(codigoSucursal);
                return Ok(inventarios.Select(i => new InventarioDTO
                {
                    Id = i.Id,
                    ProductoId = i.ProductoId,
                    NombreProducto = i.NombreProducto,
                    Cantidad = i.Cantidad,
                    StockMinimo = i.StockMinimo,
                    PrecioUnitario = i.PrecioUnitario,
                    EnAlerta = i.EnAlerta
                }));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                return StatusCode(500, "Error interno");
            }
        }

        [HttpPost("crear")]
        public async Task<ActionResult> Crear([FromBody] InventarioDTO dto)
        {
            try
            {
                var inventario = new Inventario
                {
                    ProductoId = dto.ProductoId,
                    NombreProducto = dto.NombreProducto,
                    Cantidad = dto.Cantidad,
                    StockMinimo = dto.StockMinimo,
                    PrecioUnitario = dto.PrecioUnitario,
                    CodigoSucursal = "SC" // Obtener del contexto del usuario
                };

                await _repositorio.CrearAsync(inventario);
                return CreatedAtAction(nameof(ObtenerPorSucursal), new { codigoSucursal = inventario.CodigoSucursal }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                return StatusCode(500, "Error interno");
            }
        }

        [HttpPost("hacer-pedido")]
        public async Task<ActionResult> HacerPedido([FromBody] ActualizarInventarioDTO dto)
        {
            try
            {
                var solicitud = new Core.Entidades.Solicitud
                {
                    CodigoSucursal = "SC",
                    Tipo = "Producto",
                    Descripcion = $"Pedido de {dto.NuevaCantidad} unidades",
                    Monto = 0,
                    Departamento = "Almacén",
                    Estado = "Pendiente",
                    FechaCreacion = DateTime.UtcNow
                };

                await _repositorioSolicitud.CrearAsync(solicitud);
                return Ok("Pedido solicitado al almacén");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                return StatusCode(500, "Error interno");
            }
        }
    }
}
