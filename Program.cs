using AzureServiceBus.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAzureClients(builder =>
{
    builder.AddServiceBusClient("Endpoint=sb://jkservicebuss.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=cfI/Ba3Jv/WQwkL+/n79sWUzAN31RsqcOsitGHED7sw=");
});

builder.Services.AddDbContext<BusDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ServiceBusDB")));

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

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

app.Run();
