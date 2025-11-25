using lecheriaSC.Core.Entidades;
using lecheriaSC.Core.Interfaces;
using lecheriaSC.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace lecheriaSC.Infrastructure.Repositories
{
    public class RepositorioSucursal : IRepositorioSucursal
    {
        private readonly SucursalContext _context;

        public RepositorioSucursal(SucursalContext context)
        {
            _context = context;
        }

        public async Task<Sucursal?> ObtenerPorCodigoAsync(string codigo)
        {
            return await _context.Sucursales.FirstOrDefaultAsync(s => s.Codigo == codigo);
        }

        public async Task<Sucursal?> ObtenerPorIdAsync(int id)
        {
            return await _context.Sucursales.FindAsync(id);
        }

        public async Task<IEnumerable<Sucursal>> ObtenerTodosAsync()
        {
            return await _context.Sucursales.ToListAsync();
        }

        public async Task<Sucursal> CrearAsync(Sucursal sucursal)
        {
            sucursal.FechaCreacion = DateTime.UtcNow;
            _context.Sucursales.Add(sucursal);
            await _context.SaveChangesAsync();
            return sucursal;
        }

        public async Task<bool> ActualizarAsync(Sucursal sucursal)
        {
            sucursal.FechaActualizacion = DateTime.UtcNow;
            _context.Sucursales.Update(sucursal);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var sucursal = await _context.Sucursales.FindAsync(id);
            if (sucursal == null) return false;

            _context.Sucursales.Remove(sucursal);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
