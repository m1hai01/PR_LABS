
using Kitchen.Interfaces;
using Kitchen.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IKitchen, KitchenService>();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();


// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
