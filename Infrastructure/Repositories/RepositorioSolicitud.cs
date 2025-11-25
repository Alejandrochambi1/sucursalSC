using lecheriaSC.Core.Entidades;
using lecheriaSC.Core.Interfaces;
using lecheriaSC.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace lecheriaSC.Infrastructure.Repositories
{
    public class RepositorioSolicitud : IRepositorioSolicitud
    {
        private readonly SucursalContext _context;

        public RepositorioSolicitud(SucursalContext context)
        {
            _context = context;
        }

        public async Task<Solicitud?> ObtenerPorIdAsync(int id)
        {
            return await _context.Solicitudes.FindAsync(id);
        }

        public async Task<IEnumerable<Solicitud>> ObtenerPendientesPorSucursalAsync(string codigoSucursal)
        {
            return await _context.Solicitudes
                .Where(s => s.CodigoSucursal == codigoSucursal && s.Estado == "Pendiente")
                .OrderByDescending(s => s.FechaCreacion)
                .ToListAsync();
        }

        public async Task<IEnumerable<Solicitud>> ObtenerPorSucursalAsync(string codigoSucursal)
        {
            return await _context.Solicitudes
                .Where(s => s.CodigoSucursal == codigoSucursal)
                .OrderByDescending(s => s.FechaCreacion)
                .ToListAsync();
        }

        public async Task<Solicitud> CrearAsync(Solicitud solicitud)
        {
            solicitud.FechaCreacion = DateTime.UtcNow;
            _context.Solicitudes.Add(solicitud);
            await _context.SaveChangesAsync();
            return solicitud;
        }

        public async Task<bool> ActualizarEstadoAsync(int id, string estado, string? observaciones = null)
        {
            var solicitud = await _context.Solicitudes.FindAsync(id);
            if (solicitud == null) return false;

            solicitud.Estado = estado;
            solicitud.Observaciones = observaciones;
            if (estado != "Pendiente")
            {
                solicitud.FechaAprobacion = DateTime.UtcNow;
            }

            _context.Solicitudes.Update(solicitud);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Solicitud>> ObtenerPorDepartamentoAsync(string departamento)
        {
            return await _context.Solicitudes
                .Where(s => s.Departamento == departamento && s.Estado == "Pendiente")
                .ToListAsync();
        }
    }
}
