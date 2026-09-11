using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ServiceFlow.Api.Tests;

public sealed class ServicesEndpointTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ServicesEndpointTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
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