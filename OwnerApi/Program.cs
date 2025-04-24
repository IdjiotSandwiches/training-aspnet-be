using Microsoft.EntityFrameworkCore;
using OwnerApi.ApiClients;
using OwnerApi.ApiClients.Interfaces;
using OwnerApi.Data;
using OwnerApi.Repositories;
using OwnerApi.Repositories.Interfaces;
using OwnerApi.Services;
using OwnerApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
.AddEnvironmentVariables()
.Build();

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder => builder
        .AllowAnyOrigin() // For development only, consider setting this to your frontend URL in production
        .AllowAnyMethod()
        .AllowAnyHeader()
    );
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("OwnerDatabase")));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Repository
builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();

// Service
builder.Services.AddScoped<IOwnerService, OwnerService>();

builder.Services.AddHttpClient<ISequenceApiClient, SequenceApiClient>(c => {
    c.BaseAddress = new System.Uri("http://localhost:5235/");
    c.DefaultRequestHeaders.Add("Accept", "application/json");
});

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

app.UseCors();

app.Run();
