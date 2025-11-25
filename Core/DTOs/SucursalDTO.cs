namespace lecheriaSC.Core.DTOs
{
    public class SucursalDTO
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Gerente { get; set; } = string.Empty;
        public int EmpleadosActivos { get; set; }
        public decimal SaldoPresupuesto { get; set; }
        public decimal TotalPresupuesto { get; set; }
        public bool Activa { get; set; }
    }

    public class DashboardDTO
    {
        public SucursalDTO Sucursal { get; set; } = new();
        public DatosVentasDTO Ventas { get; set; } = new();
        public DatosRRHHDTO RRHH { get; set; } = new();
        public DatosInventarioDTO Inventario { get; set; } = new();
        public DatosMarketingDTO Marketing { get; set; } = new();
        public decimal SaldoContabilidad { get; set; }
    }

    public class DatosVentasDTO
    {
        public decimal TotalMes { get; set; }
        public int TransaccionesCount { get; set; }
        public decimal Promedio { get; set; }
    }

    public class DatosRRHHDTO
    {
        public int EmpleadosActivos { get; set; }
        public int SolicitudesPendientes { get; set; }
        public decimal SumaBonosPendientes { get; set; }
    }

    public class DatosInventarioDTO
    {
        public int ProductosTotales { get; set; }
        public int ProductosEnAlerta { get; set; }
        public decimal InventarioValue { get; set; }
    }

    public class DatosMarketingDTO
    {
        public int CampañasPendientes { get; set; }
        public decimal PresupuestoPendiente { get; set; }
        public decimal PresupuestoAprobado { get; set; }
    }
}
