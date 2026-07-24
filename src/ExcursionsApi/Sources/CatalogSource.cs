using ExcursionsApi.Data;
using ExcursionsApi.Models;

namespace ExcursionsApi.Sources;

/// <summary>Marketing's source of truth for descriptive data.</summary>
public interface ICatalogSource
{
    Task<IReadOnlyList<CatalogExcursion>> GetCatalogAsync(string sailingId, CancellationToken cancellationToken = default);
}

/// <summary>Mocked: reads from the seed dataset and simulates an async backend call.</summary>
public sealed class SeedCatalogSource : ICatalogSource
{
    public async Task<IReadOnlyList<CatalogExcursion>> GetCatalogAsync(string sailingId, CancellationToken cancellationToken = default)
    {
        // Stand in for network/IO latency so callers must treat this as async.
        // await Task.Yield();
        // return Seed.Catalog.TryGetValue(sailingId, out var excursions)
        //     ? excursions
        //     : Array.Empty<CatalogExcursion>();
    }
}
