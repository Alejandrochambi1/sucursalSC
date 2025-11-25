using lecheriaSC.Core.Entidades;

namespace lecheriaSC.Core.Interfaces
{
    public interface IRepositorioReporte
    {
        Task<Reporte?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<Reporte>> ObtenerReportesDiariosPorSucursalAsync(string codigoSucursal, DateTime fecha);
        Task<Reporte?> ObtenerReporteMensualAsync(string codigoSucursal, int mes, int año);
        Task<Reporte> CrearAsync(Reporte reporte);
        Task<bool> ActualizarAsync(Reporte reporte);
    }
}
