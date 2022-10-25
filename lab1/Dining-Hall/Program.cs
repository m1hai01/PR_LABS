using Dining_Hall;
using Dining_Hall.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IHall,DiningHall>();
// Add services to the container.

builder.Services.AddControllers();

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
