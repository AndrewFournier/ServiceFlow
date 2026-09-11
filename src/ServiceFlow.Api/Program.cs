var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var services = new[]
{
    new ServiceOffering(
        Guid.NewGuid(),
        "Lawn Care",
        "Mowing, edging, and general yard cleanup.",
        75.00m,
        90),

    new ServiceOffering(
        Guid.NewGuid(),
        "Pressure Washing",
        "Exterior cleaning for driveways, patios, and siding.",
        150.00m,
        120),

    new ServiceOffering(
        Guid.NewGuid(),
        "Gutter Cleaning",
        "Removal of leaves and debris from gutters and downspouts.",
        125.00m,
        90)
};

app.MapGet("/api/services", () => Results.Ok(services))
    .WithName("GetServices");

app.Run();

record ServiceOffering(
    Guid Id,
    string Name,
    string Description,
    decimal StartingPrice,
    int EstimatedMinutes);