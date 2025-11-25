using System.Text.Json;

namespace lecheriaSC.Consumos
{
    public class ContabilidadService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ContabilidadService> _logger;

        public ContabilidadService(HttpClient httpClient, ILogger<ContabilidadService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _httpClient.BaseAddress = new Uri("https://contasantacruz-production.up.railway.app/");
        }

        public async Task<object?> ObtenerSaldoAsync(string codigoSucursal)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Saldo/sucursal/{codigoSucursal}");
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"Error obtener saldo: {response.StatusCode}");
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<object>(json);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ContabilidadService: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> AprobarGastoAsync(int solicitudId, decimal monto)
        {
            try
            {
                var data = new { solicitudId, monto };
                var content = new StringContent(JsonSerializer.Serialize(data), System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/Gastos/aprobar", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error aprobar gasto: {ex.Message}");
                return false;
            }
        }
    }
}
