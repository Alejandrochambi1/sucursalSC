using lecheriaSC.Core.Entidades;

namespace lecheriaSC.Core.Interfaces
{
    public interface IRepositorioInventario
    {
        Task<Inventario?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<Inventario>> ObtenerPorSucursalAsync(string codigoSucursal);
        Task<IEnumerable<Inventario>> ObtenerEnAlertaAsync(string codigoSucursal);
        Task<Inventario> CrearAsync(Inventario inventario);
        Task<bool> ActualizarAsync(Inventario inventario);
        Task<bool> ActualizarCantidadAsync(int productoId, int nuevaCantidad);
    }
}
