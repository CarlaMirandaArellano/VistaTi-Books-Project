using Microsoft.EntityFrameworkCore;
using VistaTi.Api.Data;
using VistaTi.Api.Services;
using System.Text.Json; 

var builder = WebApplication.CreateBuilder(args);

// 1. Configurar SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Habilitar CORS para Angular (Puerto 4200)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});



builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Camel Case 
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 3. Registrar HttpClient para consultar la API de Open Library
builder.Services.AddHttpClient();


builder.Services.AddScoped<BookService>(); // Para que el controlador pueda usar BookService

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngular");
app.UseAuthorization();
app.MapControllers();

app.Run();