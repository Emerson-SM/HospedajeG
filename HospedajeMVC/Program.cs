using HospedajeMVC.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<IClienteApiService, ClienteApiService>();
builder.Services.AddHttpClient<IEmpleadoApiService, EmpleadoApiService>();
builder.Services.AddHttpClient<IHabitacionApiService, HabitacionApiService>();
builder.Services.AddHttpClient<IReservacionApiService, ReservacionApiService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
