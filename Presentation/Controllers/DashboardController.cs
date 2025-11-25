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

        [HttpGet]
        public async Task<ActionResult<DashboardDTO>> ObtenerDashboard()
        {
            string codigoSucursal = "SC-01"; // Código fijo interno

            try
            {
                // 1. Obtener Sucursal
                var sucursal = await _repositorioSucursal.ObtenerPorCodigoAsync(codigoSucursal);

                if (sucursal == null)
                {
                    sucursal = new Core.Entidades.Sucursal
                    {
                        Codigo = codigoSucursal,
                        Nombre = "Sucursal Default",
                        Activa = true
                    };
                }

                // 2. Construir Dashboard
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
                _logger.LogError($"Error Dashboard: {ex.Message}");
                return StatusCode(500, "Error interno");
            }
        }

        // ==========================================
        // MÉTODOS AUXILIARES CORREGIDOS
        // ==========================================

        private async Task<DatosVentasDTO> ObtenerDatosVentas(string codigoSucursal)
        {
            try
            {
                // AQUÍ LA CORRECCIÓN: (DatosVentasDTO)
                // Convertimos explícitamente el 'object' que devuelve el servicio.
                var resultado = await _ventasService.ObtenerVentasPorSucursalAsync(codigoSucursal);
                return (DatosVentasDTO)resultado;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fallo Ventas: {ex.Message}");
                return new DatosVentasDTO { TransaccionesCount = 0, TotalMes = 0m, Promedio = 0m };
            }
        }

        private async Task<DatosRRHHDTO> ObtenerDatosRRHH(string codigoSucursal)
        {
            try
            {
                var solicitudes = await _repositorioSolicitud.ObtenerPendientesPorSucursalAsync(codigoSucursal);
                var bonos = solicitudes.Where(s => s.Tipo == "Bono").ToList();

                return new DatosRRHHDTO
                {
                    EmpleadosActivos = 0,
                    SolicitudesPendientes = bonos.Count,
                    SumaBonosPendientes = Convert.ToDecimal(bonos.Sum(s => s.Monto))
                };
            }
            catch
            {
                return new DatosRRHHDTO { EmpleadosActivos = 0, SolicitudesPendientes = 0, SumaBonosPendientes = 0m };
            }
        }

        private async Task<DatosInventarioDTO> ObtenerDatosInventario(string codigoSucursal)
        {
            try
            {
                var inventarios = await _repositorioInventario.ObtenerPorSucursalAsync(codigoSucursal);

                var valorTotal = inventarios.Sum(i => i.Cantidad * i.PrecioUnitario);

                return new DatosInventarioDTO
                {
                    ProductosTotales = inventarios.Count(),
                    ProductosEnAlerta = inventarios.Count(i => i.EnAlerta),
                    InventarioValue = Convert.ToDecimal(valorTotal)
                };
            }
            catch
            {
                return new DatosInventarioDTO { ProductosTotales = 0, ProductosEnAlerta = 0, InventarioValue = 0m };
            }
        }

        private async Task<DatosMarketingDTO> ObtenerDatosMarketing(string codigoSucursal)
        {
            try
            {
                var solicitudes = await _repositorioSolicitud.ObtenerPendientesPorSucursalAsync(codigoSucursal);
                var campañas = solicitudes.Where(s => s.Departamento == "Marketing").ToList();

                return new DatosMarketingDTO
                {
                    CampañasPendientes = campañas.Count,
                    PresupuestoPendiente = Convert.ToDecimal(campañas.Sum(s => s.Monto)),
                    PresupuestoAprobado = 0m
                };
            }
            catch
            {
                return new DatosMarketingDTO { CampañasPendientes = 0, PresupuestoPendiente = 0m, PresupuestoAprobado = 0m };
            }
        }

        private async Task<decimal> ObtenerSaldoContabilidad(string codigoSucursal)
        {
            try
            {
                // Convertimos el double/object del servicio a decimal
                var saldo = await _contabilidadService.ObtenerSaldoAsync(codigoSucursal);
                return Convert.ToDecimal(saldo);
            }
            catch
            {
                return 0m;
            }
        }
    }
}