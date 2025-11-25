using lecheriaSC.Core.Entidades;

namespace lecheriaSC.Core.Interfaces
{
    public interface IRepositorioSolicitud
    {
        Task<Solicitud?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<Solicitud>> ObtenerPendientesPorSucursalAsync(string codigoSucursal);
        Task<IEnumerable<Solicitud>> ObtenerPorSucursalAsync(string codigoSucursal);
        Task<Solicitud> CrearAsync(Solicitud solicitud);
        Task<bool> ActualizarEstadoAsync(int id, string estado, string? observaciones = null);
        Task<IEnumerable<Solicitud>> ObtenerPorDepartamentoAsync(string departamento);
    }
}
