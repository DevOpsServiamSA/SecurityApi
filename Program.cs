using Microsoft.EntityFrameworkCore;
using SecurityApi;
using SecurityApi.Services;

var builder = WebApplication.CreateBuilder(args);

//Configuración

AppConfig.Configuracion.Website = builder.Configuration["appConfig:Configuracion:Website"];
AppConfig.Configuracion.EnableSSLMail = Convert.ToBoolean(builder.Configuration["AppConfig:Configuracion:EnableSSLMail"]);
AppConfig.Configuracion.PasswordMail = builder.Configuration["AppConfig:Configuracion:PasswordMail"];
AppConfig.Configuracion.PuertoMail = Convert.ToInt32(builder.Configuration["AppConfig:Configuracion:PuertoMail"]);
AppConfig.Configuracion.ServidorMail = builder.Configuration["AppConfig:Configuracion:ServidorMail"];
AppConfig.Configuracion.UserMail = builder.Configuration["AppConfig:Configuracion:UserMail"];
AppConfig.Configuracion.ClientId = builder.Configuration["AppConfig:Configuracion:ClientId"];
AppConfig.Configuracion.TenantId = builder.Configuration["AppConfig:Configuracion:TenantId"];
AppConfig.Configuracion.ClientSecret = builder.Configuration["AppConfig:Configuracion:ClientSecret"];


// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>  {
    options.AddPolicy("NuevaPolitica", app => {
        app.AllowAnyOrigin()
           .AllowAnyHeader()
           .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<SecurityContext>(
    opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("Security")));

// Register IUserService with its implementation UserService
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("NuevaPolitica");
app.UseAuthorization();
app.MapControllers();
app.Run();