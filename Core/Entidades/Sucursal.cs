namespace lecheriaSC.Core.Entidades
{
    public class Sucursal
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Gerente { get; set; } = string.Empty;
        public int EmpleadosActivos { get; set; }
        public decimal SaldoPresupuesto { get; set; }
        public decimal TotalPresupuesto { get; set; }
        public bool Activa { get; set; } = true;
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }
}
