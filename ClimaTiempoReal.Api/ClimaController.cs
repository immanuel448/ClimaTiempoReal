using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

namespace ClimaTiempoReal.Api.Controllers
{

    [Route("api/[controller]")] // La ruta será api/clima (porque el controlador se llama ClimaController)
    public class ClimaController : ControllerBase
    {
        private readonly HttpClient _httpClient; // Cliente HTTP para hacer llamadas a APIs externas
        private readonly string _apiKey; // Guardará la API Key de OpenWeatherMap

        // Constructor: recibe HttpClient y la configuración (appsettings.json)
        public ClimaController(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _apiKey = config["OpenWeatherMap:ApiKey"]; // Lee la API Key desde appsettings.json
        }

        // Endpoint GET: api/clima/{ciudad}
        [HttpGet("{ciudad}")]
        public async Task<IActionResult> ObtenerClima(string ciudad)
        {
            // Verifica si la API Key está configurada
            if (string.IsNullOrEmpty(_apiKey))
                return BadRequest("API Key no configurada");

            // Construye la URL para llamar a OpenWeatherMap
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={ciudad}&appid={_apiKey}&units=metric&lang=es";

            // Hace la petición HTTP a la API de OpenWeatherMap
            var respuesta = await _httpClient.GetAsync(url);

            // Si la respuesta no fue exitosa, devuelve error
            if (!respuesta.IsSuccessStatusCode)
                return BadRequest("No se pudo obtener la información del clima");

            // Lee el contenido JSON como string
            var contenido = await respuesta.Content.ReadAsStringAsync();

            // Deserializa el JSON a un objeto genérico (puedes mapearlo a un modelo después)
            //var json = JsonSerializer.Deserialize<object>(contenido);
            var json = JsonSerializer.Deserialize<ClimaResponse>(contenido);

            // Devuelve el JSON al cliente (por ejemplo, el frontend)
            return Ok(json);
        }
    }

    //para mapear los resultados
    public class ClimaResponse
    {
        public string Name { get; set; }
        public MainInfo Main { get; set; }
        public List<WeatherInfo> Weather { get; set; }
    }

    public class MainInfo
    {
        public double Temp { get; set; }
    }

    public class WeatherInfo
    {
        public string Description { get; set; }
    }

}
