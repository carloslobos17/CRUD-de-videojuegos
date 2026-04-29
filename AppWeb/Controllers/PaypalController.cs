using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace AppWeb.Controllers
{
    public class PayPalController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _clientId;
        private readonly string _secret;
        private readonly string _baseUrl;

        public PayPalController(IConfiguration configuration)
        {
            _configuration = configuration;
            _clientId = _configuration["PayPal:ClientId"];
            _secret = _configuration["PayPal:Secret"];
            // Cambiar a https: //api-m.paypal.com para producción
            _baseUrl = "https://api-m.sandbox.paypal.com";
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> ConfirmarPago([FromForm] string orderId, [FromForm] int juegoId)
        {
            if (string.IsNullOrEmpty(orderId))
                return BadRequest(new { error = "Falta el ID de orden" });

            try
            {
                var accessToken = await GetPayPalAccessToken();

                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // 🚨 LA CLAVE ESTÁ AQUÍ:
                // PayPal requiere un JSON vacío "{}" y el tipo "application/json"
                // Si se envía null o String.Empty sin el tipo de contenido, falla.
                var content = new StringContent("{}", Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"{_baseUrl}/v2/checkout/orders/{orderId}/capture", content);

                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Aquí puedes procesar tu base de datos con el juegoId y orderId
                    //var juego = await _context.Videojuegos.FirstOrDefaultAsync(v => v.Id == juegoId);

                    return Ok(new { success = true, data = jsonResponse });
                }

                return BadRequest(new { error = "PayPal rechazó la captura", detalle = jsonResponse });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        private async Task<string> GetPayPalAccessToken()
        {
            using var client = new HttpClient();

            // 1. Leemos directamente del archivo para evitar errores de asignación
            string clientId = _configuration["PayPal:ClientId"]?.Trim();
            string secret = _configuration["PayPal:Secret"]?.Trim();
            string baseUrl = _configuration["PayPal:BaseUrl"]?.Trim();

            // 🚨 VALIDACIÓN DE EMERGENCIA: Si no lee el JSON, usamos valores fijos para probar
            if (string.IsNullOrEmpty(baseUrl))
            {
                // Esto es un "parche" para que funcione sí o sí mientras arreglas el JSON
                baseUrl = "https://api-m.sandbox.paypal.com";
            }

            if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(secret))
            {
                throw new Exception("Las credenciales ClientId o Secret están vacías en appsettings.json");
            }

            var authBytes = Encoding.UTF8.GetBytes($"{clientId}:{secret}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authBytes));

            var content = new FormUrlEncodedContent(new[]
            {
        new KeyValuePair<string, string>("grant_type", "client_credentials")
    });

            // Ahora la URL será absoluta y válida
            var response = await client.PostAsync($"{baseUrl}/v1/oauth2/token", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error de PayPal: {error}");
            }

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("access_token").GetString();
        }
    }
}