using Dining_Hall;
using Dining_Hall.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ISemaphore,DiningHall>();
// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();

// Configure the HTTP request pipeline.


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
//allow firstly start kitchen 
Thread.Sleep(1000);

Task.Run(() =>
{
    using var serviceScope = app.Services.CreateScope();
    var services = serviceScope.ServiceProvider;
    var _ = services.GetRequiredService<IHall>();
});

app.Run();
