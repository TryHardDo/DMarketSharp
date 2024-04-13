using DMarketSharp.Helpers;

namespace DMarketSharp.Endpoints;

/// <summary>
///     Get the list of items that are available for purchase on DMarket.
///     The response format is in coins (cents for USD).
///     'gameId' param values are: CS:GO - a8db, Team Fortress 2 - tf2, Dota 2 - 9a92, Rust - rust.
/// </summary>
public class MarketItems(string gameId, string currencyCode) : ApiRequestBase
{
	// Required
	public string GameId { get; } = gameId;
	public string CurrencyCode { get; } = currencyCode;

	// Optional
	public string? Title { get; init; } = null;
	public int Limit { get; init; } = 50;
	public int Offset { get; init; } = 0;
	public string OrderBy { get; init; } = "title";
	public string OrderDirection { get; init; } = "desc";
	public string? TreeFilters { get; init; } = null;
	public int PriceFrom { get; init; } = 0;
	public int PriceTo { get; init; } = 0;
	public List<string>? Types { get; init; } = null;
	public string? Cursor { get; init; }

	// Inherited
	public override HttpMethod Method => HttpMethod.Get;
	public override string RelativePath => "/exchange/v1/market/items";

	public override object? UriQueryParams => new
	{
		gameId = GameId,
		title = Title,
		limit = Limit,
		offset = Offset,
		orderBy = OrderBy,
		orderDir = OrderDirection,
		treeFilters = TreeFilters,
		currency = CurrencyCode,
		priceFrom = PriceFrom,
		priceTo = PriceTo,
		types = Types == null ? null : string.Join(',', Types),
		cursor = Cursor
	};
}