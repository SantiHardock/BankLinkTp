using BankLink.Context;
using BankLink.Interfaces; 
using BankLink.Services;   
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Autenticación de JWT
var jwtKey = Environment.GetEnvironmentVariable("Jwt__Key") ?? builder.Configuration["Jwt:Key"];
var jwtIssuer = Environment.GetEnvironmentVariable("Jwt__Issuer") ?? builder.Configuration["Jwt:Issuer"];
var jwtAudience = Environment.GetEnvironmentVariable("Jwt__Audience") ?? builder.Configuration["Jwt:Audience"];

var key = Encoding.UTF8.GetBytes(jwtKey ?? "super_secreto_super_largo_para_banklink_api_2026!");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,
            ValidateAudience = true,
            ValidAudience = jwtAudience,
            ValidateLifetime = true, 
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddAuthorization();

// Conexión a nuestra base de datos SQLite (Blindada para leer DefaultConnection o BankLinkDb)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                       ?? builder.Configuration.GetConnectionString("BankLinkDb") 
                       ?? "Data Source=banklink.db";

builder.Services.AddDbContext<BankLinkDbContext>(options => 
    options.UseSqlite(connectionString));

builder.Services.AddControllers();

// Inyección de dependencias de nuestros servicios CRUD
builder.Services.AddScoped<IClienteService, ClienteDbService>();
builder.Services.AddScoped<ICuentaService, CuentaDbService>();
builder.Services.AddScoped<IMovimientoService, MovimientoDbService>();
builder.Services.AddScoped<IBancoExternoService, BancoExternoDbService>();

// HttpClient Tipado para integrarnos con los otros grupos y con la API Key por defecto
builder.Services.AddHttpClient<ITransferenciaService, TransferenciaService>(client =>
{
    var apiKey = builder.Configuration["ApiExterna:ApiKey"] ?? builder.Configuration["ApiKey"];
    if (!string.IsNullOrEmpty(apiKey))
    {
        client.DefaultRequestHeaders.Add("X-API-Key", apiKey);
    }
});

// Swagger para la documentación visual
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Creación automática de la base de datos y sus tablas al iniciar
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<BankLinkDbContext>();
    context.Database.EnsureCreated(); 
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
//app.UseHttpsRedirection();

app.MapControllers();
app.Run();