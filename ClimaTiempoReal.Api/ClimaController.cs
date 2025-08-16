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

        public ClimaController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("{ciudad}")]
        public async Task<IActionResult> ObtenerClima(string ciudad)
        {
            // Aquí deberías usar tu propia API Key de OpenWeatherMap
            string apiKey = "36be44fb38b1e5ab6de112ddbffddb0e";
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={ciudad}&appid={apiKey}&units=metric&lang=es";

            var respuesta = await _httpClient.GetAsync(url);

            if (!respuesta.IsSuccessStatusCode)
                return BadRequest("No se pudo obtener la información del clima");

            var contenido = await respuesta.Content.ReadAsStringAsync();

            var json = JsonSerializer.Deserialize<object>(contenido);

            return Ok(json);
        }
    }
}
