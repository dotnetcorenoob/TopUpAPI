
using System;
using System.Data.SqlClient;
using System.Data;
using topUpApi.Services;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.SwaggerUI;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ITopUpRepository, TopUpRepository>();


builder.Services.AddScoped<ITopUpService, TopUpService>();

builder.Services.AddScoped<IBalanceService, ExternalBalanceService>();

//builder.Services.AddDbContext<AppDbContext>(options =>
//options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddTransient<IDbConnection>((sp) => new SqlConnection(Configuration.GetConnectionString("DefaultConnection")));
// Add services to the container.
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

Log.Logger = new LoggerConfiguration()
          .WriteTo.Console()
          .WriteTo.File("C://Work/logs/log.txt", rollingInterval: RollingInterval.Day) // Adjust the log file path as needed
          .CreateLogger();

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddSerilog();
});

builder.Services.AddAuthentication("Bearer")
           .AddIdentityServerAuthentication(options =>
           {
               options.Authority = "https://localhost:5001"; // Replace with your IdentityServer4 authority URL
               options.RequireHttpsMetadata = false; // For testing purposes, consider using HTTPS in a production environment
               options.ApiName = "topupapi";
           });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseSwagger();

// Enable middleware to serve Swagger UI
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TopUpApi v1");
    c.RoutePrefix = "swagger";
    c.DocExpansion(DocExpansion.List);
});
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
