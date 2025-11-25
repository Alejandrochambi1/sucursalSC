using lecheriaSC.Core.DTOs;
using lecheriaSC.Core.Entidades;

namespace lecheriaSC.Core.Mapeadores
{
    public class MapeadorSucursal
    {
        public static SucursalDTO EntidadADTO(Sucursal sucursal)
        {
            return new SucursalDTO
            {
                Id = sucursal.Id,
                Codigo = sucursal.Codigo,
                Nombre = sucursal.Nombre,
                Gerente = sucursal.Gerente,
                EmpleadosActivos = sucursal.EmpleadosActivos,
                SaldoPresupuesto = sucursal.SaldoPresupuesto,
                TotalPresupuesto = sucursal.TotalPresupuesto,
                Activa = sucursal.Activa
            };
        }

        public static Sucursal DTOAEntidad(SucursalDTO dto)
        {
            return new Sucursal
            {
                Id = dto.Id,
                Codigo = dto.Codigo,
                Nombre = dto.Nombre,
                Gerente = dto.Gerente,
                EmpleadosActivos = dto.EmpleadosActivos,
                SaldoPresupuesto = dto.SaldoPresupuesto,
                TotalPresupuesto = dto.TotalPresupuesto,
                Activa = dto.Activa,
                FechaCreacion = DateTime.UtcNow
            };
        }
    }
}
