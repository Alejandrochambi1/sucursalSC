using System.Text.Json;

namespace lecheriaSC.Consumos
{
    public class VentasService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<VentasService> _logger;

        public VentasService(HttpClient httpClient, ILogger<VentasService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _httpClient.BaseAddress = new Uri("https://ventas-production-06b1.up.railway.app/");
        }

        public async Task<object?> ObtenerVentasPorSucursalAsync(string codigoSucursal, DateTime? desde = null, DateTime? hasta = null)
        {
            try
            {
                var query = $"api/Ventas/sucursal/{codigoSucursal}";
                if (desde.HasValue && hasta.HasValue)
                {
                    query += $"?desde={desde:yyyy-MM-dd}&hasta={hasta:yyyy-MM-dd}";
                }

                var response = await _httpClient.GetAsync(query);
                if (!response.IsSuccessStatusCode) return null;

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<object>(json);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en VentasService: {ex.Message}");
                return null;
            }
        }
    }
}
