namespace lecheriaSC.Core.Entidades
{
    public class Inventario
    {
        public int Id { get; set; }
        public string CodigoSucursal { get; set; } = string.Empty;
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public int StockMinimo { get; set; } = 10;
        public decimal PrecioUnitario { get; set; }
        public bool EnAlerta { get; set; }
        public DateTime UltimaActualizacion { get; set; }
    }
}
