# Stage 1 — Scaffolding (.NET 8) — **BROKEN**

⚠️ **This project currently has compiler errors and will not build.** Your task is to fix the errors first before proceeding to Stage 2.

**What's included**
- ASP.NET Core minimal API (`src/ExcursionsApi`) + xUnit (`tests/ExcursionsApi.Tests`).
- Two mock data-source services: `ICatalogSource`, `IInventorySource`
  (`src/ExcursionsApi/Sources/`), registered in DI.
- Sample data with intentional inconsistencies (`src/ExcursionsApi/Data/data-sample.json`).
- An **incomplete** seed class (`src/ExcursionsApi/Data/Seed.cs`) — you need to
  complete it so it exposes the data from `data-sample.json` to the sources.
- The shared type contract (`src/ExcursionsApi/Models/Types.cs`).
- A **stub** endpoint at `GET /sailings/{id}/excursions` that returns both
  sources raw and un-reconciled.
- A **stub** test suite (`tests/ExcursionsApi.Tests/SourcesTests.cs`) with smoke
  tests + skipped placeholders for step 1.

**What's not included:** reconciliation, UI, holds.

## Step 1: Fix Compiler Errors

Run the build command to identify and fix all compiler errors:
```bash
dotnet build
```

Once the build succeeds with no errors, you can proceed to Stage 2.

## Step 2: Complete the Seed Class

`src/ExcursionsApi/Data/Seed.cs` is currently empty. Complete it using the data
located in `src/ExcursionsApi/Data/data-sample.json` (catalog + inventory for
sailing `S1`), so that `ICatalogSource` and `IInventorySource` have data to serve.

## Step 3: Verify the Build

Once compiler errors are fixed:
```bash
dotnet test
dotnet run --project src/ExcursionsApi    # http://localhost:5300
# curl http://localhost:5300/sailings/S1/excursions
```

**Next:** Stage 2 — reconcile the two sources.
