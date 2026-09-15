namespace ServiceFlow.Api.Domain;

public sealed class ServiceOffering
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public required string Name { get; set; }

    public required string Description { get; set; }

    public decimal StartingPrice { get; set; }

    public int EstimatedMinutes { get; set; }

    public bool IsActive { get; set; } = true;
}