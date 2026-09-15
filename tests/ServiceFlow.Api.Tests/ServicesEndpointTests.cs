using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ServiceFlow.Api.Data;

namespace ServiceFlow.Api.Tests;

public sealed class ServicesEndpointTests
    : IClassFixture<ServicesApiFactory>
{
    private readonly HttpClient _client;

    public ServicesEndpointTests(ServicesApiFactory factory)
    {
        _client = factory.CreateClient();

        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider
            .GetRequiredService<ServiceFlowDbContext>();
        dbContext.Database.EnsureCreated();
    }

    [Fact]
    public async Task GetServices_ReturnsAvailableServices()
    {
        var response = await _client.GetAsync("/api/services");

        response.EnsureSuccessStatusCode();

        var services =
            await response.Content.ReadFromJsonAsync<List<ServiceResponse>>();

        Assert.NotNull(services);
        Assert.Equal(3, services.Count);
        Assert.Contains(
            services,
            service => service.Name == "Pressure Washing");
    }

    private sealed record ServiceResponse(
        Guid Id,
        string Name,
        string Description,
        decimal StartingPrice,
        int EstimatedMinutes);
}

public sealed class ServicesApiFactory : WebApplicationFactory<Program>
{
    private readonly string _databaseName = $"ServiceFlowTests-{Guid.NewGuid()}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<ServiceFlowDbContext>();
            services.RemoveAll<DbContextOptions<ServiceFlowDbContext>>();
            services.RemoveAll<
                IDbContextOptionsConfiguration<ServiceFlowDbContext>>();

            services.AddDbContext<ServiceFlowDbContext>(options =>
                options.UseInMemoryDatabase(_databaseName));
        });
    }
}
