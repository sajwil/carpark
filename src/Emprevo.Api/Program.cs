using Emprevo.Api.Services.Rates;
using Emprevo.Api.Services.Rates.Calculators;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
}).AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
});

builder.Services.AddScoped<ICalculationEngineService, CalculationEngineService>();
builder.Services.AddScoped<EarlybirdRateCalculator>();
builder.Services.AddScoped<NightRateCalculator>();
builder.Services.AddScoped<WeekendRateCalculator>();
builder.Services.AddScoped<StandardRateCalculator>();

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
