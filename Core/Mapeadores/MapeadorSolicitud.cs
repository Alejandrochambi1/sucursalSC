using lecheriaSC.Core.DTOs;
using lecheriaSC.Core.Entidades;

namespace lecheriaSC.Core.Mapeadores
{
    public class MapeadorSolicitud
    {
        public static SolicitudResponseDTO EntidadADTO(Solicitud solicitud)
        {
            return new SolicitudResponseDTO
            {
                Id = solicitud.Id,
                Tipo = solicitud.Tipo,
                Descripcion = solicitud.Descripcion,
                Monto = solicitud.Monto,
                Estado = solicitud.Estado,
                Departamento = solicitud.Departamento,
                FechaCreacion = solicitud.FechaCreacion
            };
        }

        public static Solicitud DTOAEntidad(CrearSolicitudDTO dto)
        {
            return new Solicitud
            {
                CodigoSucursal = dto.CodigoSucursal,
                Tipo = dto.Tipo,
                Descripcion = dto.Descripcion,
                Monto = dto.Monto,
                Departamento = dto.Departamento,
                IdDepartamento = dto.IdDepartamento,
                Estado = "Pendiente",
                FechaCreacion = DateTime.UtcNow
            };
        }
    }
}
