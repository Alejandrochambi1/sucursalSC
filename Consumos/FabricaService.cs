using System.Text.Json;

namespace lecheriaSC.Consumos
{
    public class FabricaService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<FabricaService> _logger;

        public FabricaService(HttpClient httpClient, ILogger<FabricaService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _httpClient.BaseAddress = new Uri("https://test-production-d2fa.up.railway.app/");
        }

        public async Task<object?> ObtenerCatalogoProductosAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/productos");
                if (!response.IsSuccessStatusCode) return null;

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<object>(json);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en FabricaService: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> HacerPedidoAsync(object solicitud)
        {
            try
            {
                var content = new StringContent(JsonSerializer.Serialize(solicitud), System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/SolicitudDemanda", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error hacer pedido: {ex.Message}");
                return false;
            }
        }
    }
}
