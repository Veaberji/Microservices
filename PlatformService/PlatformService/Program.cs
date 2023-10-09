using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMemory"));
services.AddScoped<IPlatformRepository, PlatformRepository>();
services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
services.AddHttpClient<ICommandDataClient, CommandDataClient>();

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

await PrepareDB.PopulateAsync(app);

app.Run();
