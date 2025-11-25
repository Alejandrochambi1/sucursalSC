namespace lecheriaSC.Core.DTOs
{
    public class CrearSolicitudDTO
    {
        public string CodigoSucursal { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public string Departamento { get; set; } = string.Empty;
        public int? IdDepartamento { get; set; }
    }

    public class SolicitudResponseDTO
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string Departamento { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
    }

    public class AprobarSolicitudDTO
    {
        public int IdSolicitud { get; set; }
        public bool Aprobar { get; set; }
        public string? Observaciones { get; set; }
    }
}
