using ExcursionsApi.Models;

namespace ExcursionsApi.Data;

/*
 * Seed dataset for sailing "S1". The two sources are deliberately inconsistent
 * so the reconciliation policy has something to reconcile:
 *
 *  - EX-100  Clean happy path (present + consistent in both sources).
 *  - EX-200  Present in catalog, MISSING from inventory  → can't be sold.
 *  - EX-300  Price mismatch (catalog 109 USD vs inventory 119 USD).
 *  - EX-400  Capacity/sold discrepancy: oversold (sold 10 > capacity 8).
 *  - EX-500  Sold out exactly (capacity == sold).
 *  - EX-600  Departs within 48h of "today" (2026-06-26); local departure time
 *            with no offset — fodder for the timezone trap + the 48h change request.
 *
 * Today, for reference, is 2026-06-25.
 */
public static class Seed
{
    
}
