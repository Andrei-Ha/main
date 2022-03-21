using Exadel.OfficeBooking.Api.DTO.BookingDto;
using Exadel.OfficeBooking.Api.Interfaces;
using Exadel.OfficeBooking.Api.Services;
using Exadel.OfficeBooking.Domain.Bookings;
using Exadel.OfficeBooking.EF;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Configuration.AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);
var connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // specifies whether the issuer will be verified when validating the token
            ValidateIssuer = true,
            // a string representing the publisher
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            // whether the consumer of the token will be validated
            ValidateAudience = true,
            // setting consumer token
            ValidAudience = builder.Configuration["Jwt:Audience"],
            // whether lifetime will be validated
            ValidateLifetime = true,
            // security key setting
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            // security key validation
            ValidateIssuerSigningKey = true,
        };
    });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IOfficeService, OfficeService>();
builder.Services.AddScoped<IMapService, MapService>();
builder.Services.AddScoped<IWorkplaceService, WorkplaceService>();
builder.Services.AddScoped<IParkingPlaceService, ParkingPlaceService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IEmailService, EmailService>();
var app = builder.Build();

//custom adapter for Booking
TypeAdapterConfig<Booking, GetBookingDto>.NewConfig()
    .Map(dest => dest.WorkplaceId, src => src.Workplace.Id)
    .Map(dest => dest.OfficeId, src => src.Workplace.Map.OfficeId)
    .Map(dest => dest.UserId, src => src.User.Id)
    .Map(dest => dest.WorkplaceName, src => src.Workplace.Name)
    .Map(dest => dest.FloorNumber, src => src.Workplace.Map.FloorNumber)
    .Map(dest => dest.OfficeName, src => $"{src.Workplace.Map.Office.Name} {src.Workplace.Map.Office.Address}" )
    .Map(dest => dest.ParkingPlaceNumber, src => src.ParkingPlace.PlaceNumber);


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
