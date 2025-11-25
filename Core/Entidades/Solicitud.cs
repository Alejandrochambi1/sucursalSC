namespace lecheriaSC.Core.Entidades
{
    public class Solicitud
    {
        public int Id { get; set; }
        public string CodigoSucursal { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty; // "Producto", "Presupuesto", "Bono", "Campaña"
        public string Descripcion { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public string Estado { get; set; } = "Pendiente"; // "Pendiente", "Aprobada", "Rechazada"
        public string Departamento { get; set; } = string.Empty; // "RRHH", "Marketing", "Almacén", "Contabilidad"
        public string? Observaciones { get; set; }
        public int? IdDepartamento { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaAprobacion { get; set; }
    }
}
