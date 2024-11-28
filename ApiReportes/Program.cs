using Microsoft.EntityFrameworkCore;
using ApiReportes.Context;
using ApiReportes.Repositories;
using ApiReportes.Services;

var builder = WebApplication.CreateBuilder(args);

//cadena de conexion
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//servicio para la conexion a la base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

// Configuración del puerto
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5017); // Cambia 5017 por el puerto que desees
});

// Habilitar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Add services to the container.

builder.Services.AddControllers();

//registro de clienteRepository
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();

//registro de clienteService
builder.Services.AddScoped<IClienteService, ClienteService>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Usar CORS
app.UseCors("AllowAllOrigins");

app.UseAuthorization();

app.MapControllers();

app.Run();
