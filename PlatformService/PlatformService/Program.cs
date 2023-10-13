using Microsoft.EntityFrameworkCore;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.SyncDataServices.Grpc;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

if (builder.Environment.IsDevelopment())
{
    services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMemory"));
}
else
{
    services.AddDbContext<AppDbContext>(
        opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("Platforms")));
}
services.AddScoped<IPlatformRepository, PlatformRepository>();
services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
services.AddSingleton<IMessageBusClient, MessageBusClient>();
services.AddGrpc();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapGrpcService<GrpcPlatformService>();
app.MapGet(
    "/protos/platforms.proto",
    async context => await context.Response.WriteAsync(await File.ReadAllTextAsync("Protos/platforms.proto")));

await PrepareDB.PopulateAsync(app, app.Environment.IsProduction());

app.Run();
