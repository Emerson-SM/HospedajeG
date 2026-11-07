using HospedajeAccesoDatos.Repositories;
using HospedajeAccesoDatos.Repositories.Interfaces;
using HospedajeAccesoDatos.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IEmpleadoRepository, EmpleadoRepository>();
builder.Services.AddScoped<IHabitacionesRepository, HabitacionRepository>();
builder.Services.AddScoped<IReservacionesRepository, ReservacionRepository>();

builder.Services.AddDbContext<HospedajeContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Conexion")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();