using System.Text.Json;
using System.Text.Json.Serialization;

namespace ExcursionsApi.Models;

/*
 * Shared types for the Shore Excursion Booking slice.
 *
 * Two backend systems own different slices of the truth:
 *  - Catalog (marketing): descriptive data + the customer-facing price.
 *  - Inventory (operations): availability, status, and the local departure time.
 *
 * They are intentionally inconsistent. The reconciler's job is to merge them
 * into a single shape a UI can trust — and to surface, never hide, conflicts.
 */

/// <summary>Marketing's source of truth: descriptive data.</summary>
public sealed record CatalogExcursion(
    string ExcursionId,
    string Title,
    string Description,
    IReadOnlyList<string> Images,
    /// Customer-facing price. Marketing owns this.
    decimal Price,
    /// ISO 4217 currency code, e.g. "USD".
    string Currency);

/// <summary>Serialized lowercase on the wire: "open" | "closed" | "cancelled".</summary>
[JsonConverter(typeof(LowerCaseEnumConverter<InventoryStatus>))]
public enum InventoryStatus
{
    Open,
    Closed,
    Cancelled,
}

/// <summary>Operations' source of truth: availability.</summary>
public sealed record InventoryExcursion(
    string ExcursionId,
    int Capacity,
    int Sold,
    InventoryStatus Status,
    /// <summary>
    /// Local wall-clock departure time at the port, with NO timezone offset
    /// (e.g. "2026-07-10T09:00:00"). Treat as local-to-the-port — converting it
    /// to UTC naively is one of the planted traps in this exercise. Kept as a
    /// string on purpose: parsing it into DateTime/DateTimeOffset too early is
    /// exactly how the offset gets invented.
    /// </summary>
    string LocalDepartureTime,
    /// <summary>
    /// Price as recorded by the ops/point-of-sale system. May disagree with the
    /// catalog price — that disagreement is a reconciliation decision, not a bug
    /// to silently paper over.
    /// </summary>
    decimal Price,
    string Currency);

/// <summary>
/// First-class availability state. "Can't be sold" is a state, not a deletion.
/// Serialized as "AVAILABLE" | "SOLD_OUT" | "UNAVAILABLE".
/// </summary>
[JsonConverter(typeof(UpperSnakeCaseEnumConverter<ExcursionAvailability>))]
public enum ExcursionAvailability
{
    Available,
    SoldOut,
    Unavailable,
}

/// <summary>Serialized as "NO_INVENTORY" | "PRICE_MISMATCH" | "CURRENCY_MISMATCH" | "OVERSOLD".</summary>
[JsonConverter(typeof(UpperSnakeCaseEnumConverter<ReconciliationWarningCode>))]
public enum ReconciliationWarningCode
{
    NoInventory,
    PriceMismatch,
    CurrencyMismatch,
    Oversold,
}

public sealed record ReconciliationWarning(ReconciliationWarningCode Code, string Detail);

/// <summary>The merged shape the API returns and the UI consumes.</summary>
public sealed record ReconciledExcursion(
    string ExcursionId,
    // Descriptive — from catalog.
    string Title,
    string Description,
    IReadOnlyList<string> Images,
    decimal Price,
    string Currency,
    // Availability — derived from inventory.
    ExcursionAvailability Availability,
    int? Capacity,
    int? Sold,
    int? AvailableCount,
    string? LocalDepartureTime,
    /// Conflicts surfaced honestly rather than silently resolved.
    IReadOnlyList<ReconciliationWarning> Warnings);

/// <summary>"Open" → "open". Single-word members, so camelCase == lowercase.</summary>
internal sealed class LowerCaseEnumConverter<TEnum> : JsonStringEnumConverter<TEnum>
    where TEnum : struct, Enum
{
    public LowerCaseEnumConverter() : base(JsonNamingPolicy.CamelCase) { }
}

/// <summary>"SoldOut" → "SOLD_OUT".</summary>
internal sealed class UpperSnakeCaseEnumConverter<TEnum> : JsonStringEnumConverter<TEnum>
    where TEnum : struct, Enum
{
    public UpperSnakeCaseEnumConverter() : base(JsonNamingPolicy.SnakeCaseUpper) { }
}
