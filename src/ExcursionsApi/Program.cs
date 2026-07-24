using ExcursionsApi.Sources;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ICatalogSource, SeedCatalogSource>();
builder.Services.AddSingleton<IInventorySource, SeedInventorySource>();

var app = builder.Build();

/*
 * GET /sailings/{id}/excursions  — STARTER STUB.
 *
 * Wiring only: both mock sources are fetched in parallel and returned raw and
 * un-reconciled, so you can see what each side reports. They intentionally
 * disagree.
 *
 * TODO (Build step 1): replace { catalog, inventory } with a single reconciled
 * list and a documented policy (which source wins per field, how an unsellable
 * excursion is represented, what happens on a price mismatch).
 */
app.MapGet("/sailings/{id}/excursions", async (string id, ICatalogSource catalogSource, IInventorySource inventorySource, CancellationToken cancellationToken) =>
{
   // Fetch both sources in parallel, then return them raw and un-reconciled.
});

app.Run();

// Exposed so integration tests can host the app via WebApplicationFactory<Program>.
public partial class Program { }
