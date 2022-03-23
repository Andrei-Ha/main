using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Exadel.OfficeBooking.TelegramApi.StateMachine;
using Exadel.OfficeBooking.TelegramApi.Steps;
using System;
using Exadel.OfficeBooking.TelegramApi.EF;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);
builder.Configuration.AddEnvironmentVariables();

var connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];
builder.Services.AddDbContext<TelegramDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddSingleton<Exadel.OfficeBooking.TelegramApi.TelegramBot>();
builder.Services.AddScoped<StateMachine>();
builder.Services.AddScoped<StateMachineStep, Start>();
builder.Services.AddScoped<StateMachineStep, ManageForChoice>();
builder.Services.AddScoped<StateMachineStep, OfficeReportChoice>();
builder.Services.AddScoped<StateMachineStep, ActionChoice>();
builder.Services.AddScoped<StateMachineStep, CityChoice>();
builder.Services.AddScoped<StateMachineStep, OfficeChoice>();
builder.Services.AddScoped<StateMachineStep, DatesChoice>();
builder.Services.AddScoped<StateMachineStep, ParkingChoice>();
builder.Services.AddScoped<StateMachineStep, ParkingPlaceSpecifications>();
builder.Services.AddScoped<StateMachineStep, SpecParamChoice>();
builder.Services.AddScoped<StateMachineStep, FloorChoice>();
builder.Services.AddScoped<StateMachineStep, WorkplaceChoice>();
builder.Services.AddScoped<StateMachineStep, ConfirmBooking>();
builder.Services.AddScoped<StateMachineStep, BookingChoice>();
builder.Services.AddScoped<StateMachineStep, EditingChoise>();
builder.Services.AddScoped<StateDb>();
builder.Services.AddHttpClient("WebAPI", c =>
{
    c.BaseAddress = new Uri(builder.Configuration["WebApi"]);
    c.DefaultRequestHeaders.Add("Accept", "*/*");
    c.DefaultRequestHeaders.Add("User-Agent", "TelegramApi");
});
builder.Services.AddScoped<StateMachineStep, Template>();

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

using (var scope = app.Services.CreateScope())
    await scope.ServiceProvider.GetRequiredService<TelegramDbContext>().Database.MigrateAsync();

app.Run();
