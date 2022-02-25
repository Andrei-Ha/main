using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Exadel.OfficeBooking.TelegramApi.StateMachine;
using Exadel.OfficeBooking.TelegramApi.Steps;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddSingleton<Exadel.OfficeBooking.TelegramApi.TelegramBot>();
builder.Services.AddScoped<StateMachine>();
builder.Services.AddScoped<StateMachineStep,Start>();
builder.Services.AddScoped<StateMachineStep, ActionChoise>();
builder.Services.AddHttpClient();

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
