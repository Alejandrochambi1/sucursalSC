namespace lecheriaSC.Core.Entidades
{
    public class Reporte
    {
        public int Id { get; set; }
        public string CodigoSucursal { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty; // "Diario", "Mensual"
        public DateTime Fecha { get; set; }
        public decimal VentasTotal { get; set; }
        public decimal CostosTotal { get; set; }
        public decimal Ganancia { get; set; }
        public int EmpleadosTrabajos { get; set; }
        public decimal PresupuestoGastado { get; set; }
        public string JsonData { get; set; } = string.Empty;
    }
}
