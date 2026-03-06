var builder = WebApplication.CreateBuilder(args);
// Crea el objeto principal para configurar la aplicaci�n (builder)

builder.Services.AddControllers();
// Registra los controladores de la API en el contenedor de servicios

builder.Services.AddEndpointsApiExplorer();
// Habilita la exploraci�n de endpoints, necesario para Swagger

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
// Configura una pol�tica CORS llamada "PermitirTodo"

var app = builder.Build();
// Construye la aplicaci�n con todo lo configurado arriba

app.UseCors("PermitirTodo");
// Activa la pol�tica CORS para que pueda usarse en las peticiones

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Si el entorno es de desarrollo, habilita Swagger y su interfaz visual

app.UseHttpsRedirection();
// Fuerza a que las peticiones HTTP se redirijan autom�ticamente a HTTPS

app.UseAuthorization();
// Habilita el middleware de autorizaci�n (aunque no haya autenticaci�n a�n)

app.MapControllers();
// Mapea los controladores para que las rutas funcionen (ej: api/clima)

app.Run();
// Arranca la aplicaci�n y la deja escuchando en el puerto configurado
