using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

namespace ClimaTiempoReal.Api.Controllers
{
    // Ruta base para este controlador, que ser� api/clima
    [Route("api/[controller]")]
    public class ClimaController : ControllerBase
    {
        private readonly HttpClient _httpClient; // Cliente HTTP para hacer llamadas a APIs externas
        private readonly string _apiKey; // Guardar� la API Key de OpenWeatherMap

        // Constructor: Recibe HttpClient y la configuraci�n (appsettings.json)
        public ClimaController(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _apiKey = config["OpenWeatherMap:ApiKey"]; // Lee la API Key desde el archivo appsettings.json
        }

        // Endpoint para obtener el clima actual de una ciudad: api/clima/{ciudad}
        [HttpGet("{ciudad}")]
        public async Task<IActionResult> ObtenerClima(string ciudad)
        {
            // Verifica si la API Key est� configurada correctamente
            if (string.IsNullOrEmpty(_apiKey))
                return BadRequest("API Key no configurada");

            // Construye la URL para llamar a la API de OpenWeatherMap usando la ciudad y la API Key
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={ciudad}&appid={_apiKey}&units=metric&lang=es";

            // Hace la petici�n HTTP a la API de OpenWeatherMap
            var respuesta = await _httpClient.GetAsync(url);

            // Si la respuesta no fue exitosa, devuelve un error
            if (!respuesta.IsSuccessStatusCode)
                return BadRequest("No se pudo obtener la informaci�n del clima");

            // Lee el contenido de la respuesta como string (JSON)
            var contenido = await respuesta.Content.ReadAsStringAsync();

            // Configuraci�n para deserializar el JSON con nombres de propiedades insensibles a may�sculas
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            // Deserializa el JSON a un objeto espec�fico para el clima
            var json = JsonSerializer.Deserialize<ClimaResponse>(contenido, options);

            // Devuelve el resultado en formato JSON al cliente (por ejemplo, el frontend)
            return Ok(json);
        }

        // Endpoint para obtener el pron�stico del clima para los pr�ximos 5 d�as: api/clima/pronostico/{ciudad}
        [HttpGet("pronostico/{ciudad}")]
        public async Task<IActionResult> ObtenerPronostico(string ciudad)
        {
            // Construye la URL para obtener el pron�stico de 5 d�as
            string url = $"https://api.openweathermap.org/data/2.5/forecast?q={ciudad}&appid={_apiKey}&units=metric&lang=es";

            // Hace la petici�n HTTP a la API para obtener el pron�stico
            var respuesta = await _httpClient.GetAsync(url);

            // Si la respuesta no fue exitosa, devuelve un error
            if (!respuesta.IsSuccessStatusCode)
                return BadRequest("No se pudo obtener el pron�stico");

            // Lee el contenido de la respuesta como string (JSON)
            var contenido = await respuesta.Content.ReadAsStringAsync();

            // Deserializa el contenido JSON a un objeto gen�rico (podr�as mapearlo a un modelo m�s espec�fico)
            var json = JsonSerializer.Deserialize<object>(contenido);

            // Devuelve el pron�stico en formato JSON al cliente
            return Ok(json);
        }

        // Lista est�tica para guardar las ciudades favoritas (en memoria)
        private static List<string> _ciudadesFavoritas = new();

        // Endpoint POST para agregar una ciudad a la lista de favoritos
        [HttpPost("favoritos/{ciudad}")]
        public IActionResult AgregarFavorito(string ciudad)
        {
            // Si la ciudad no est� en la lista de favoritos, la agrega
            if (!_ciudadesFavoritas.Contains(ciudad))
                _ciudadesFavoritas.Add(ciudad);

            // Devuelve la lista actualizada de ciudades favoritas
            return Ok(_ciudadesFavoritas);
        }

        // Endpoint GET para obtener la lista de ciudades favoritas
        [HttpGet("favoritos")]
        public IActionResult ObtenerFavoritos()
        {
            // Devuelve la lista de ciudades favoritas
            return Ok(_ciudadesFavoritas);
        }
    }

    // Clase para mapear la respuesta del clima
    public class ClimaResponse
    {
        public string Name { get; set; } // Nombre de la ciudad
        public MainInfo Main { get; set; } // Informaci�n principal del clima (como la temperatura)
        public List<WeatherInfo> Weather { get; set; } // Descripci�n del clima (ej. soleado, lluvioso)
    }

    // Clase para mapear la informaci�n principal (ej. temperatura)
    public class MainInfo
    {
        public double Temp { get; set; } // Temperatura en grados Celsius
    }

    // Clase para mapear la informaci�n del clima (descripci�n)
    public class WeatherInfo
    {
        public string Description { get; set; } // Descripci�n del clima (ej. cielo despejado)
    }

}
