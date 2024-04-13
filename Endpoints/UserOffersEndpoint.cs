using DMarketSharp.Helpers;

namespace DMarketSharp.Endpoints;

/// <summary>
///     Get the list of offers of the current user for further management (remove from sale/change price, etc.).
///     The price amount format is in USD i.e. 0.5 is 50 cents.
///     'gameId' param values are: CS:GO - a8db, Team Fortress 2 - tf2, Dota 2 - 9a92, Rust - rust.
/// </summary>
public class UserOffersEndpoint : EndpointBase
{
	public string? GameId { get; init; } = null;
	public string Status { get; init; } = "OfferStatusDefault";
	public string SortType { get; init; } = "UserOffersSortTypeDefault";
	public double PriceFrom { get; init; } = 0;
	public double PriceTo { get; init; } = 0;
	public string? FilterCurrency { get; init; } = null;
	public int Offset { get; init; } = 0;
	public int Limit { get; init; } = 50;
	public string? Cursor { get; init; } = null;

	public override HttpMethod Method => HttpMethod.Get;
	public override string RelativePath => "/marketplace-api/v1/user-offers";

	public override object? UriQueryParams => new Dictionary<string, object?>
	{
		{ "GameID", GameId },
		{ "Status", Status },
		{ "SortType", SortType },
		{ "BasicFilters.PriceFrom", PriceFrom == 0 || FilterCurrency == null ? null : PriceFrom },
		{ "BasicFilters.PriceTo", PriceTo == 0 || FilterCurrency == null ? null : PriceTo },
		{ "BasicFilters.Currency", FilterCurrency },
		{ "Offset", Offset == 0 ? null : Offset },
		{ "Limit", Limit },
		{ "Cursor", Cursor }
	};

	public override object? BodyContent => null;
}