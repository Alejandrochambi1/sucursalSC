using lecheriaSC.Core.Entidades;
using lecheriaSC.Core.Interfaces;
using lecheriaSC.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace lecheriaSC.Infrastructure.Repositories
{
    public class RepositorioInventario : IRepositorioInventario
    {
        private readonly SucursalContext _context;

        public RepositorioInventario(SucursalContext context)
        {
            _context = context;
        }

        public async Task<Inventario?> ObtenerPorIdAsync(int id)
        {
            return await _context.Inventarios.FindAsync(id);
        }

        public async Task<IEnumerable<Inventario>> ObtenerPorSucursalAsync(string codigoSucursal)
        {
            return await _context.Inventarios
                .Where(i => i.CodigoSucursal == codigoSucursal)
                .ToListAsync();
        }

        public async Task<IEnumerable<Inventario>> ObtenerEnAlertaAsync(string codigoSucursal)
        {
            return await _context.Inventarios
                .Where(i => i.CodigoSucursal == codigoSucursal && i.Cantidad < i.StockMinimo)
                .ToListAsync();
        }

        public async Task<Inventario> CrearAsync(Inventario inventario)
        {
            inventario.UltimaActualizacion = DateTime.UtcNow;
            inventario.EnAlerta = inventario.Cantidad < inventario.StockMinimo;
            _context.Inventarios.Add(inventario);
            await _context.SaveChangesAsync();
            return inventario;
        }

        public async Task<bool> ActualizarAsync(Inventario inventario)
        {
            inventario.UltimaActualizacion = DateTime.UtcNow;
            inventario.EnAlerta = inventario.Cantidad < inventario.StockMinimo;
            _context.Inventarios.Update(inventario);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ActualizarCantidadAsync(int productoId, int nuevaCantidad)
        {
            var inventario = await _context.Inventarios.FirstOrDefaultAsync(i => i.ProductoId == productoId);
            if (inventario == null) return false;

            inventario.Cantidad = nuevaCantidad;
            inventario.EnAlerta = nuevaCantidad < inventario.StockMinimo;
            return await ActualizarAsync(inventario);
        }
    }
}
