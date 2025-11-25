using lecheriaSC.Core.DTOs;
using lecheriaSC.Core.Interfaces;
using lecheriaSC.Consumos;
using Microsoft.AspNetCore.Mvc;

namespace lecheriaSC.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IRepositorioSucursal _repositorioSucursal;
        private readonly IRepositorioSolicitud _repositorioSolicitud;
        private readonly IRepositorioInventario _repositorioInventario;
        private readonly ContabilidadService _contabilidadService;
        private readonly RRHHService _rrhhService;
        private readonly VentasService _ventasService;
        private readonly MarketingService _marketingService;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(
            IRepositorioSucursal repositorioSucursal,
            IRepositorioSolicitud repositorioSolicitud,
            IRepositorioInventario repositorioInventario,
            ContabilidadService contabilidadService,
            RRHHService rrhhService,
            VentasService ventasService,
            MarketingService marketingService,
            ILogger<DashboardController> logger)
        {
            _repositorioSucursal = repositorioSucursal;
            _repositorioSolicitud = repositorioSolicitud;
            _repositorioInventario = repositorioInventario;
            _contabilidadService = contabilidadService;
            _rrhhService = rrhhService;
            _ventasService = ventasService;
            _marketingService = marketingService;
            _logger = logger;
        }

        [HttpGet("{codigoSucursal}")]
        public async Task<ActionResult<DashboardDTO>> ObtenerDashboard(string codigoSucursal)
        {
            try
            {
                var sucursal = await _repositorioSucursal.ObtenerPorCodigoAsync(codigoSucursal);
                if (sucursal == null) return NotFound("Sucursal no encontrada");

                var dashboard = new DashboardDTO
                {
                    Sucursal = Core.Mapeadores.MapeadorSucursal.EntidadADTO(sucursal),
                    Ventas = await ObtenerDatosVentas(codigoSucursal),
                    RRHH = await ObtenerDatosRRHH(codigoSucursal),
                    Inventario = await ObtenerDatosInventario(codigoSucursal),
                    Marketing = await ObtenerDatosMarketing(codigoSucursal),
                    SaldoContabilidad = await ObtenerSaldoContabilidad(codigoSucursal)
                };

                return Ok(dashboard);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error dashboard: {ex.Message}");
                return StatusCode(500, "Error interno");
            }
        }

        private async Task<DatosVentasDTO> ObtenerDatosVentas(string codigoSucursal)
        {
            var datos = await _ventasService.ObtenerVentasPorSucursalAsync(codigoSucursal);
            return new DatosVentasDTO { TransaccionesCount = 0, TotalMes = 0, Promedio = 0 };
        }

        private async Task<DatosRRHHDTO> ObtenerDatosRRHH(string codigoSucursal)
        {
            var solicitudes = await _repositorioSolicitud.ObtenerPendientesPorSucursalAsync(codigoSucursal);
            var bonos = solicitudes.Where(s => s.Tipo == "Bono").ToList();

            return new DatosRRHHDTO
            {
                EmpleadosActivos = 0,
                SolicitudesPendientes = bonos.Count,
                SumaBonosPendientes = bonos.Sum(s => s.Monto)
            };
        }

        private async Task<DatosInventarioDTO> ObtenerDatosInventario(string codigoSucursal)
        {
            var inventarios = await _repositorioInventario.ObtenerPorSucursalAsync(codigoSucursal);
            var enAlerta = inventarios.Count(i => i.EnAlerta);

            return new DatosInventarioDTO
            {
                ProductosTotales = inventarios.Count(),
                ProductosEnAlerta = enAlerta,
                InventarioValue = inventarios.Sum(i => i.Cantidad * i.PrecioUnitario)
            };
        }

        private async Task<DatosMarketingDTO> ObtenerDatosMarketing(string codigoSucursal)
        {
            var solicitudes = await _repositorioSolicitud.ObtenerPendientesPorSucursalAsync(codigoSucursal);
            var campañas = solicitudes.Where(s => s.Departamento == "Marketing").ToList();

            return new DatosMarketingDTO
            {
                CampañasPendientes = campañas.Count,
                PresupuestoPendiente = campañas.Sum(s => s.Monto)
            };
        }

        private async Task<decimal> ObtenerSaldoContabilidad(string codigoSucursal)
        {
            var saldo = await _contabilidadService.ObtenerSaldoAsync(codigoSucursal);
            return 0; // Parsear respuesta de Contabilidad
        }
    }
}
