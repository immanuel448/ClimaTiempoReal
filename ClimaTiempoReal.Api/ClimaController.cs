using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Text.Json;

namespace ClimaTiempoReal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClimaController : ControllerBase
    {
        private readonly HttpClient _http;

        public ClimaController(HttpClient http)
        {
            _http = http;
        }

        [HttpGet("{ciudad}")]
        public async Task<IActionResult> ObtenerClima(string ciudad)
        {
            // 1. Obtener coordenadas
            var coordenadasUrl = $"https://nominatim.openstreetmap.org/search?q={ciudad}&format=json&limit=1";
            var coordenadasJson = await _http.GetStringAsync(coordenadasUrl);
            var coordenadas = JsonSerializer.Deserialize<List<Coordenada>>(coordenadasJson);

            if (coordenadas == null || !coordenadas.Any())
                return NotFound("Ciudad no encontrada");

            var lat = coordenadas[0].Lat;
            var lon = coordenadas[0].Lon;

            // 2. Obtener clima
            var climaUrl = $"https://api.open-meteo.com/v1/forecast?latitude={lat}&longitude={lon}&current=temperature_2m,relative_humidity_2m,wind_speed_10m&daily=temperature_2m_max,temperature_2m_min&forecast_days=5&timezone=auto";
            var clima = await _http.GetFromJsonAsync<object>(climaUrl);

            return Ok(clima);
        }
    }

    public class Coordenada
    {
        public string Lat { get; set; }
        public string Lon { get; set; }
    }
}

