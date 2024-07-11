using Emprevo.Api.Services.Rates;
using Emprevo.Api.Services.Rates.Calculators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICalculationEngine, CalculationEngine>();

builder.Services.AddScoped<IRateCalculator, EarlybirdRateCalculator>();
builder.Services.AddScoped<IRateCalculator, NightRateCalculator>();
builder.Services.AddScoped<IRateCalculator, WeekendRateCalculator>();
builder.Services.AddScoped<IRateCalculator, StandardRateCalculator>();


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

await app.RunAsync();
