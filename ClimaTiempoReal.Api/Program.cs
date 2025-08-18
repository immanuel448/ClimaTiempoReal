var builder = WebApplication.CreateBuilder(args);
// Crea el objeto principal para configurar la aplicación (builder)

builder.Services.AddControllers();
// Registra los controladores de la API en el contenedor de servicios

builder.Services.AddEndpointsApiExplorer();
// Habilita la exploración de endpoints, necesario para Swagger

builder.Services.AddSwaggerGen();
// Registra Swagger para generar documentación de la API

builder.Services.AddHttpClient();
// Registra un HttpClient que se puede inyectar en controladores para consumir APIs externas

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodo", policy =>
    {
        policy.AllowAnyOrigin()    // Permite peticiones desde cualquier origen
              .AllowAnyMethod()    // Permite cualquier método HTTP (GET, POST, PUT, DELETE, etc.)
              .AllowAnyHeader();   // Permite cualquier encabezado HTTP
    });
});
// Configura una política CORS llamada "PermitirTodo"

var app = builder.Build();
// Construye la aplicación con todo lo configurado arriba

app.UseCors("PermitirTodo");
// Activa la política CORS para que pueda usarse en las peticiones

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Si el entorno es de desarrollo, habilita Swagger y su interfaz visual

app.UseHttpsRedirection();
// Fuerza a que las peticiones HTTP se redirijan automáticamente a HTTPS

app.UseAuthorization();
// Habilita el middleware de autorización (aunque no haya autenticación aún)

app.MapControllers();
// Mapea los controladores para que las rutas funcionen (ej: api/clima)

app.Run();
// Arranca la aplicación y la deja escuchando en el puerto configurado
