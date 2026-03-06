# ClimaTiempoReal API

API desarrollada con ASP.NET Core que consume la API de OpenWeatherMap para obtener información del clima y pronóstico por ciudad. Incluye un frontend simple en HTML y JavaScript que consulta los endpoints de la API.

## Tecnologías
- ASP.NET Core Web API
- .NET
- OpenWeatherMap API
- Swagger
- HTML + JavaScript (Fetch API)

## Endpoints

GET /api/clima/{ciudad}  
Obtiene el clima actual de una ciudad.

GET /api/clima/pronostico/{ciudad}  
Obtiene el pronóstico del clima para los próximos días.

POST /api/clima/favoritos/{ciudad}  
Agrega una ciudad a la lista de favoritos.

GET /api/clima/favoritos  
Devuelve la lista de ciudades favoritas.

## Configuración

Agregar la API Key de OpenWeatherMap en `appsettings.json`.

OpenWeatherMap:
  ApiKey: TU_API_KEY

## Ejecutar proyecto

dotnet run

Luego abrir en el navegador:

https://localhost:{puerto}/swagger
