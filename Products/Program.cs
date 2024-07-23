using MassTransit;
using Products.Rabbitmq;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Eureka;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;

services.AddServiceDiscovery(o => o.UseEureka());

services.AddSingleton<IRabbitMqService, RabbitMqService>();

services.AddControllers();

services.AddEndpointsApiExplorer();

services.AddCors();

// Add logging
services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseCors(b => b
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            );

app.Run();
