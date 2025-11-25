using System.Text.Json;

namespace lecheriaSC.Consumos
{
    public class MarketingService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<MarketingService> _logger;

        public MarketingService(HttpClient httpClient, ILogger<MarketingService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _httpClient.BaseAddress = new Uri("https://marketinglechesc-production.up.railway.app/");
        }

        public async Task<object?> ObtenerCampañasPendientesAsync(string codigoSucursal)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Campañas/pendientes/{codigoSucursal}");
                if (!response.IsSuccessStatusCode) return null;

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<object>(json);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en MarketingService: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> AprobarCampañaAsync(int campaniaId, bool aprobar)
        {
            try
            {
                var data = new { campaniaId, aprobar };
                var content = new StringContent(JsonSerializer.Serialize(data), System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/Campañas/aprobar", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error aprobar campaña: {ex.Message}");
                return false;
            }
        }
    }
}
