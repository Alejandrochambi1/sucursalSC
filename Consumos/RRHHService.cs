using System.Text.Json;

namespace lecheriaSC.Consumos
{
    public class RRHHService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<RRHHService> _logger;

        public RRHHService(HttpClient httpClient, ILogger<RRHHService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _httpClient.BaseAddress = new Uri("https://brrhh-production.up.railway.app/");
        }

        public async Task<object?> ObtenerEmpleadosPorSucursalAsync(string codigoSucursal)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Empleados/sucursal/{codigoSucursal}");
                if (!response.IsSuccessStatusCode) return null;

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<object>(json);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en RRHHService: {ex.Message}");
                return null;
            }
        }

        public async Task<object?> ObtenerSolicitudesBonos(string codigoSucursal)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Bonos/solicitudes/{codigoSucursal}");
                if (!response.IsSuccessStatusCode) return null;

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<object>(json);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error obtener bonos: {ex.Message}");
                return null;
            }
        }
    }
}
