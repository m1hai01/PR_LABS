using Waiter;
using Waiter.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IWaiter, WaiterService>();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();