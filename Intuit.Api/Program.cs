using System.Reflection;
using Intuit.Api.Data;
using Intuit.Api.Data.Repository;
using Intuit.Api.Interfaces;
using Intuit.Api.Logging;
using Intuit.Api.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configurar Serilog
LoggerConfig.ConfigureLogging();
builder.Host.UseSerilog();

Log.Information("Iniciando la aplicacion...");

// Configurar la conexi�n a PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("PostgreSQL");

builder.Services.AddDbContext<IntuitDBContext>(
   options => options.UseNpgsql(connectionString)
);

builder.Services.AddSingleton<ILogService<LogService>, LogService>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IClientService, ClientService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // incluir comentarios XML
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
});

//configuracion de los CORS
builder.Services.AddCors(
    options =>
    {
        options.AddDefaultPolicy(
            policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            }
            );
    }
);

var app = builder.Build();

// Migraciones automáticas
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<IntuitDBContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.yaml", "Intuit.Api");
});

// Middleware
app.UseSerilogRequestLogging(); // Middleware para loggear las solicitudes HTTP

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors();
app.MapControllers();

app.Run();
