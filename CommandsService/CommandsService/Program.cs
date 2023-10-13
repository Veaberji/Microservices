using CommandsService.AsyncDataServices;
using CommandsService.Data;
using CommandsService.EventProcessing;
using CommandsService.SyncDataServices.Grpc;
using Microsoft.EntityFrameworkCore;
using PlatformService.Data;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
services.AddScoped<ICommandRepository, CommandRepository>();
services.AddSingleton<IEventProcessor, EventProcessor>();
services.AddScoped<IPlatformDataClient, PlatformDataClient>();

services.AddHostedService<MessageBusSubscriber>();

services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMemory"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await PrepareDB.PopulateAsync(app, app.Environment.IsProduction());

app.Run();
