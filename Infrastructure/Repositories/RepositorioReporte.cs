using lecheriaSC.Core.Entidades;
using lecheriaSC.Core.Interfaces;
using lecheriaSC.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace lecheriaSC.Infrastructure.Repositories
{
    public class RepositorioReporte : IRepositorioReporte
    {
        private readonly SucursalContext _context;

        public RepositorioReporte(SucursalContext context)
        {
            _context = context;
        }

        public async Task<Reporte?> ObtenerPorIdAsync(int id)
        {
            return await _context.Reportes.FindAsync(id);
        }

        public async Task<IEnumerable<Reporte>> ObtenerReportesDiariosPorSucursalAsync(string codigoSucursal, DateTime fecha)
        {
            return await _context.Reportes
                .Where(r => r.CodigoSucursal == codigoSucursal && r.Tipo == "Diario" && r.Fecha.Date == fecha.Date)
                .ToListAsync();
        }

        public async Task<Reporte?> ObtenerReporteMensualAsync(string codigoSucursal, int mes, int año)
        {
            return await _context.Reportes
                .FirstOrDefaultAsync(r => r.CodigoSucursal == codigoSucursal && 
                                         r.Tipo == "Mensual" && 
                                         r.Fecha.Month == mes && 
                                         r.Fecha.Year == año);
        }

        public async Task<Reporte> CrearAsync(Reporte reporte)
        {
            _context.Reportes.Add(reporte);
            await _context.SaveChangesAsync();
            return reporte;
        }

        public async Task<bool> ActualizarAsync(Reporte reporte)
        {
            _context.Reportes.Update(reporte);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
