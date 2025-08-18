using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

namespace ClimaTiempoReal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClimaController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public ClimaController(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _apiKey = config["OpenWeatherMap:ApiKey"]; // lee desde appsettings.json
        }

        [HttpGet("{ciudad}")]
        public async Task<IActionResult> ObtenerClima(string ciudad)
        {
            if (string.IsNullOrEmpty(_apiKey))
                return BadRequest("API Key no configurada");

            string url = $"https://api.openweathermap.org/data/2.5/weather?q={ciudad}&appid={_apiKey}&units=metric&lang=es";

            var respuesta = await _httpClient.GetAsync(url);

            if (!respuesta.IsSuccessStatusCode)
                return BadRequest("No se pudo obtener la información del clima");

            var contenido = await respuesta.Content.ReadAsStringAsync();

            var json = JsonSerializer.Deserialize<object>(contenido);

            return Ok(json);
        }
    }
}
