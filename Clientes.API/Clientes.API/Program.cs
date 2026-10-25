using Clientes.Application.Services.Implementations;
using Clientes.Application.Services.Interface;
using Clientes.Core.Events;
using Clientes.Core.Repository;
using Clientes.Infrastructure.Context;
using Clientes.Infrastructure.Messaging;
using Clientes.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ClienteDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<RabbitMqSettings>(
    builder.Configuration.GetSection(RabbitMqSettings.SectionName));

builder.Services.AddSingleton<IRabbitMqConnection, RabbitMqConnection>();

builder.Services.AddScoped<IClienteEventPublisher, ClienteEventPublisher>();

builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IClienteService, ClienteService>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
