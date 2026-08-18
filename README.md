# Stage 1 — Scaffolding (.NET 8) — **BROKEN**

⚠️ **This project currently has compiler errors and will not build.** Your task is to fix the errors first before proceeding to Stage 2.

**What's included**
- ASP.NET Core minimal API (`src/ExcursionsApi`).
- Two mock data-source services: `ICatalogSource`, `IInventorySource`
  (`src/ExcursionsApi/Sources/`), registered in DI.
- Sample data with intentional inconsistencies (`src/ExcursionsApi/Data/data-sample.json`).
- An **incomplete** seed class (`src/ExcursionsApi/Data/Seed.cs`) — you need to
  complete it so it exposes the data from `data-sample.json` to the sources.
- The shared type contract (`src/ExcursionsApi/Models/Types.cs`).
- A **stub** endpoint at `GET /sailings/{id}/excursions` that returns both
  sources raw and un-reconciled.

**What's not included:** a test project, reconciliation, UI, holds.

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
dotnet build
dotnet run --project src/ExcursionsApi    # http://localhost:5300
# curl http://localhost:5300/sailings/S1/excursions
```

You should see all six catalog excursions and the five inventory records, raw and
un-reconciled. How you verify your work from here — by hand, or with a test
project you add yourself — is your call.

---

## What's next

- **Stage 2 — Reconcile the two sources.** Merge catalog and inventory into the
  single `ReconciledExcursion` shape the UI consumes, with a documented policy:
  which source wins per field, how an unsellable excursion is represented, and
  what happens on a price mismatch. Spec delivered separately.
- **Stage 3 — Change request, on the clock.** A new requirement lands once Stage 2
  passes. See below.

## Stage 3 — Change request: booking cutoff

> Ops just told us guests can't book an excursion inside **48 hours** of departure.
> Add it.

Budget: ~15 minutes, on top of a working Stage 2.

**Acceptance**
- `GET /sailings/S1/excursions` reports EX-600 (departs `2026-06-26T08:00:00`,
  local to the port) as not bookable, with a reason a UI can render.
- The other five excursions — all departing in July — are unaffected.
- The reason is visible in the response, not filtered out of it. An excursion the
  guest can't buy is still an excursion they can see.
- The cutoff decision is reachable without waiting for real time to pass.

**Deliberately unspecified**

`LocalDepartureTime` is local wall-clock at the port with no UTC offset, and it's
a `string` on purpose (see the remarks on `InventoryExcursion` in
`src/ExcursionsApi/Models/Types.cs`). Nothing in the data says which timezone that
is, and "48 hours before departure" isn't well-defined without it. Parsing the
string against the server's clock resolves the ambiguity by accident — in favour
of whatever machine happens to be running the code. Decide what to do about the
missing input, and make the decision legible in the code.

**Where it lands is your call**

`ExcursionAvailability` and `ReconciliationWarning` already exist. Whether
"booking closed" is a new availability state, a new warning code, both, or
something else is a design decision — as is whether the rule belongs in the
reconciler, the endpoint, or its own unit. Say why you put it where you put it.
