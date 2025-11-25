using lecheriaSC.Core.DTOs;
using lecheriaSC.Core.Entidades;

namespace lecheriaSC.Core.Interfaces
{
    public interface IRepositorioSucursal
    {
        Task<Sucursal?> ObtenerPorCodigoAsync(string codigo);
        Task<Sucursal?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<Sucursal>> ObtenerTodosAsync();
        Task<Sucursal> CrearAsync(Sucursal sucursal);
        Task<bool> ActualizarAsync(Sucursal sucursal);
        Task<bool> EliminarAsync(int id);
    }
}
