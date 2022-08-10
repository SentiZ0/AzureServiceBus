using BusServiceReceiver;
using BusServiceReceiver.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Azure;
using MediatR;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddSingleton<IServiceBusConsumer, ServiceBusConsumer>();
builder.Services.AddSingleton<IProcessData, ProcessData>();

builder.Services.AddDbContext<BusDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ServiceBusDB")));

builder.Services.AddMemoryCache();
builder.Services.AddSession();

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

var bus = app.Services.GetService<IServiceBusConsumer>();
bus.RegisterOnMessageHandlerAndReceiveMessages().GetAwaiter().GetResult();

app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
