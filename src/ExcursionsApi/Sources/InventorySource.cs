using ExcursionsApi.Data;
using ExcursionsApi.Models;

namespace ExcursionsApi.Sources;

/// <summary>Operations' source of truth for availability.</summary>
public interface IInventorySource
{
    Task<IReadOnlyList<InventoryExcursion>> GetInventoryAsync(string sailingId, CancellationToken cancellationToken = default);
}

/// <summary>Mocked: reads from the seed dataset and simulates an async backend call.</summary>
public sealed class SeedInventorySource : IInventorySource
{
    public async Task<IReadOnlyList<InventoryExcursion>> GetInventoryAsync(string sailingId, CancellationToken cancellationToken = default)
    {
       
    }
}
