using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Exadel.OfficeBooking.TelegramApi.StateMachine;
using Exadel.OfficeBooking.TelegramApi.Steps;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddSingleton<Exadel.OfficeBooking.TelegramApi.TelegramBot>();
builder.Services.AddScoped<StateMachine>();
builder.Services.AddScoped<StateMachineStep,Start>();
builder.Services.AddScoped<StateMachineStep, ActionChoise>();
builder.Services.AddScoped<StateMachineStep, CityChoise>();
builder.Services.AddScoped<StateMachineStep, OfficeChoise>();
builder.Services.AddScoped<StateMachineStep, DatesChoise>();
builder.Services.AddHttpClient("WebAPI", c =>
{
    c.BaseAddress = new Uri("https://localhost:7110/api/");
    c.DefaultRequestHeaders.Add("Accept", "*/*");
    c.DefaultRequestHeaders.Add("User-Agent", "TelegramApi");
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
