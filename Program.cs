using Microsoft.EntityFrameworkCore;
using SecurityApi.Services;

var builder = WebApplication.CreateBuilder(args);

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