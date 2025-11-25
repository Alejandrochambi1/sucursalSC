namespace lecheriaSC.Core.DTOs
{
    public class InventarioDTO
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public int StockMinimo { get; set; }
        public decimal PrecioUnitario { get; set; }
        public bool EnAlerta { get; set; }
    }

    public class ActualizarInventarioDTO
    {
        public int ProductoId { get; set; }
        public int NuevaCantidad { get; set; }
        public int StockMinimo { get; set; } = 10;
    }
}
