using ExcursionsApi.Sources;
using Xunit;

namespace ExcursionsApi.Tests;

/// <summary>
/// Stub test runner. Proves the harness works against the mock sources, and
/// sketches (via skipped placeholders) what Build step 1's reconciliation tests
/// must cover. Replace/extend these once the reconciler exists.
/// </summary>
public class SourcesTests
{
    private readonly ICatalogSource _catalog = new SeedCatalogSource();
    private readonly IInventorySource _inventory = new SeedInventorySource();

    [Fact]
    public async Task GetCatalog_returns_seeded_excursions_for_S1()
    {
        var catalog = await _catalog.GetCatalogAsync("S1");

        Assert.NotEmpty(catalog);
        Assert.False(string.IsNullOrEmpty(catalog[0].ExcursionId));
    }

    [Fact]
    public async Task GetInventory_returns_seeded_availability_for_S1()
    {
        var inventory = await _inventory.GetInventoryAsync("S1");

        Assert.NotEmpty(inventory);
        Assert.True(inventory[0].Capacity > 0);
    }

    [Fact]
    public async Task The_two_sources_disagree_that_is_the_whole_point()
    {
        var catalogTask = _catalog.GetCatalogAsync("S1");
        var inventoryTask = _inventory.GetInventoryAsync("S1");
        await Task.WhenAll(catalogTask, inventoryTask);

        var catalogIds = (await catalogTask).Select(c => c.ExcursionId).ToHashSet();
        var inventoryIds = (await inventoryTask).Select(i => i.ExcursionId).ToHashSet();

        // At least one excursion is in catalog but missing from inventory.
        Assert.Contains(catalogIds, id => !inventoryIds.Contains(id));
    }
}

/// <summary>TODO — Build step 1: reconciliation.</summary>
public class ReconciliationTodoTests
{
    [Fact(Skip = "TODO — Build step 1")]
    public void Descriptive_fields_come_from_catalog_availability_from_inventory() { }

    [Fact(Skip = "TODO — Build step 1")]
    public void A_catalog_only_excursion_is_kept_as_UNAVAILABLE_not_dropped() { }

    [Fact(Skip = "TODO — Build step 1")]
    public void A_price_mismatch_is_surfaced_not_silently_resolved() { }

    [Fact(Skip = "TODO — Build step 1")]
    public void Oversold_never_yields_a_negative_availableCount() { }
}
