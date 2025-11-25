namespace lecheriaSC.Core.DTOs
{
    public class ReporteDiarioDTO
    {
        public string CodigoSucursal { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public decimal VentasTotal { get; set; }
        public decimal CostosTotal { get; set; }
        public int EmpleadosTrabajos { get; set; }
        public decimal PresupuestoGastado { get; set; }
    }

    public class ReporteResponseDTO
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public decimal VentasTotal { get; set; }
        public decimal CostosTotal { get; set; }
        public decimal Ganancia { get; set; }
        public int EmpleadosTrabajos { get; set; }
    }
}
