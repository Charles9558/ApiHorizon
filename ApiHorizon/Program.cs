
using ApiHorizon.Data;
using ApiHorizon.EndPoints;
using ApiHorizon.Services;
using Dapper;
using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors();

var configuration = app.Services.GetService<IConfiguration>();
DbConnection.Initilize(configuration);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapGet("/", () => "Hello world");
app.MapEmailDTOEndPoint();
app.MapComgeEndPoint();
app.MapPointageEndPoint();
app.MapDepartementEndPoint();
app.MapPosteEndPoint();
app.MapPersonnelEndPoint();

app.Run();


