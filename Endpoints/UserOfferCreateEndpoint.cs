using DMarketSharp.Helpers;
using DMarketSharp.Utils;

namespace DMarketSharp.Endpoints;

/// <summary>
///     Batch offers creation.
///     As a result, the selected asset is locked and the newly-created offer appears in the Market and on Sale tabs.
///     The price amount format is in USD, i.e. 0.5 is 50 cents.
/// </summary>
public class UserOfferCreateEndpoint : EndpointBase
{
	public Dictionary<string, object> PricedAssetsCache { get; } = [];

	public override HttpMethod Method => HttpMethod.Post;
	public override string RelativePath => "/marketplace-api/v1/user-offers/create";
	public override object? UriQueryParams => null;

	public override object? BodyContent => new
	{
		Offers = PricedAssetsCache.Values
	};

	public bool AddToDict(string assetId, int priceInCents,
		string currencyType = SiteConstants.CurrencyTypes.UnitedStatesDollar)
	{
		var assetObj = new
		{
			AssetID = assetId,
			Price = new
			{
				Currency = currencyType,
				Amount = priceInCents
			}
		};

		return PricedAssetsCache.TryAdd(assetId, assetObj);
	}

	public bool RemoveFromDict(string assetId)
	{
		return PricedAssetsCache.Remove(assetId);
	}
}