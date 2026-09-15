using Microsoft.EntityFrameworkCore;
using ServiceFlow.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var connectionString =
    builder.Configuration.GetConnectionString("ServiceFlow")
    ?? throw new InvalidOperationException(
        "The ServiceFlow database connection string is missing.");

builder.Services.AddDbContext<ServiceFlowDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("Frontend");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/api/services", async (ServiceFlowDbContext dbContext) =>
    Results.Ok(await dbContext.Services
        .AsNoTracking()
        .Where(service => service.IsActive)
        .ToListAsync()))
    .WithName("GetServices");

app.Run();

public partial class Program;
